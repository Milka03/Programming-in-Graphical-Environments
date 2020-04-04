// Bejeweled.cpp : Defines the entry point for the application.
//

#include "pch.h"
#include "framework.h"
#include "Bejeweled.h"
#include "Resource.h"
#include "time.h"
#include <string>
#include <ctime>
#include <vector>
#include <algorithm>


#define MAX_LOADSTRING 100
#define ScreenX GetSystemMetrics(SM_CXSCREEN)
#define ScreenY GetSystemMetrics(SM_CYSCREEN)
#define MARGIN 10
#define MAX_BOARD_SIZE 12
#define PARTICLE_NUM 100

#define TIMER_NEWGAME 1
#define TIMER_FALLING 2
#define TIMER_MOUSE 3
#define TIMER_PARTICLES 4
#define MOUSE_INTERVAL 50
#define NEWGAME_INTERVAL 40
#define FALLING_INTERVAL 300
#define PARTICLES_INTERVAL 20

//initial variables
const unsigned int Numbers[] = { 8, 10, 12 };      //number of tiles in one row/column depending on game mode
const unsigned int Sizes[] = { 80, 70, 60 };       //sizes of tiles 
int tile_size = Sizes[0];						//current tile size (initially set to small board mode)
int tile_number = Numbers[0];					//current number of tiles in one row/column (initially set to small board mode)
int WIDTH = (Numbers[0] + 1)*MARGIN + Numbers[0] * Sizes[0] + 15;   //current width of main window depending on game mode (initially small)
int HEIGHT = (Numbers[0] + 1)*MARGIN + Numbers[0] * Sizes[0] + 60;   //current height of main window depending on game mode (initially small)

const int bufSize = 50;           
wchar_t buf[bufSize];					//buffer to store the text of main window title
int startX = (ScreenX - WIDTH) / 2;    //initial coordinates of main window
int startY = (ScreenY - HEIGHT) / 2;

//Structure for storing data about one tile
typedef struct
{
	HWND ChildWnd;
	COLORREF BkGndColor;
	bool crosspattern;
} ChildInfo;

ChildInfo Tiles[MAX_BOARD_SIZE][MAX_BOARD_SIZE]{};   //matrix of tiles on board
bool TilesToDestroy[MAX_BOARD_SIZE][MAX_BOARD_SIZE]{}; //matrix to store the state of each tile - when true tile is 'destroyed' and background switched to hatch

COLORREF Colors[6] = { RGB(255, 255, 0)/*yellow*/, RGB(102, 255, 51)/*green*/, RGB(0, 255, 255)/*seledin*/,
						RGB(0, 0, 255) /*blue*/, RGB(255, 0, 255) /*pink*/, RGB(255, 0, 0)/*red*/ };

HWND selectedWindowHwnd = NULL;  //handle to first window selected by user
HWND windowToSwap;				//handle to second selected window
static bool newgame = false;    //tells if board is during newgame recoloring
static bool start = true;         //tiles are initially gray
static bool falling_tiles = false;  //tells if tiles are currently falling
static bool blockScreen = false;   //blocking screen while falling tiles
static bool pause = false;       //pause between falling tiles to check uf there are no more matches

// Global Variables:
HINSTANCE hInst;                            // current instance
WCHAR szTitle[MAX_LOADSTRING];              // The title bar text
WCHAR szWindowClass[MAX_LOADSTRING];        // the main window class name
HWND hWndMain;                              //handle to main window
HWND hwndParticle;							//handle to overlay window
HMENU hMenu;								//main window menu
HMENU SysMenu;								//system menu


//Particles variables
typedef struct
{
	POINT position;
	double direction_x;
	double direction_y;
	COLORREF colour;
} Particle;

bool debug = false; //tells whether the debug mode is turned on
int particle_size = tile_size / MARGIN;  //stores the current size of one animation particle (initially = 8)
std::vector<Particle> particles; //vector storing information about the position, direction and color of each particle



// Forward declarations of functions included in this code module:
ATOM                MyRegisterClass(HINSTANCE hInstance);
ATOM                MyRegisterTileClass(HINSTANCE hInstance);
ATOM				MyRegisterParticleClass(HINSTANCE hInstance);
BOOL                InitInstance(HINSTANCE, int);
LRESULT CALLBACK    WndProc(HWND, UINT, WPARAM, LPARAM);
LRESULT CALLBACK    TileWndProc(HWND, UINT, WPARAM, LPARAM);
LRESULT CALLBACK    ParticleWndProc(HWND, UINT, WPARAM, LPARAM);
INT_PTR CALLBACK    About(HWND, UINT, WPARAM, LPARAM);
//Initialization of board/tile features
void		AddTiles();			 //initialize the child windows' position and features
void		ColorTiles();			//color all tiles on board
void		ChangeBoardSize(int newSize);  //change board size and initialize tiles for different size
void		updateTile(HWND hWnd, RECT rc, int n);   //resize specified tile when mouse hovers over it
COLORREF	FindColor(HWND window);			//return color assigned to specified child window
bool		FindCrossPattern(HWND window);     //return if specified child window has cross pattern
//Game logic
bool		FindMove();			//check if there are any tiles to remove from board 
bool		ThreeInRow();		//find a horizontal match 
bool		ThreeInColumn();		//find a vertical match 
int			HorizontalFall(int ile, int row, int col);
void		VerticalFall(int row, int col);			//column of tiles falling one place down starting from specified row and column
void		DestroyTiles(int ile, int row, int col, bool vertical);    //change color of matched tiles to cross pattern
bool		IsMoveCorrect(HWND window1, HWND window2);    //check if user's move is correct
//Particles' movement
void		CreateParticles();		//create particles in place of matched tiles
void		RemoveParticles();		//remove particles that reached the end of the screen


int APIENTRY wWinMain(_In_ HINSTANCE hInstance,
                     _In_opt_ HINSTANCE hPrevInstance,
                     _In_ LPWSTR    lpCmdLine,
                     _In_ int       nCmdShow)
{
    UNREFERENCED_PARAMETER(hPrevInstance);
    UNREFERENCED_PARAMETER(lpCmdLine);

    // TODO: Place code here.

    // Initialize global strings
    LoadStringW(hInstance, IDS_APP_TITLE, szTitle, MAX_LOADSTRING);
    LoadStringW(hInstance, IDC_BEJEWELED, szWindowClass, MAX_LOADSTRING);
	MyRegisterClass(hInstance);
	MyRegisterTileClass(hInstance);
	MyRegisterParticleClass(hInstance);

	// Perform application initialization:
	if (!InitInstance(hInstance, nCmdShow))
	{
		return FALSE;
	}

	HACCEL hAccelTable = LoadAccelerators(hInstance, MAKEINTRESOURCE(IDC_BEJEWELED));

	MSG msg;
	hMenu = GetMenu(hWndMain);
	CheckMenuItem(hMenu, ID_BOARDSIZE_SMALL, MF_BYCOMMAND | MF_CHECKED);

	SysMenu = GetSystemMenu(hWndMain, false);
	ModifyMenuA(SysMenu, SC_RESTORE, MF_BYCOMMAND | MF_GRAYED, SC_RESTORE, (LPCSTR)"&Przywróæ");
	ModifyMenuA(SysMenu, SC_SIZE, MF_BYCOMMAND | MF_GRAYED, SC_SIZE, (LPCSTR)"&Rozmiar");
	ModifyMenuA(SysMenu, SC_MAXIMIZE, MF_BYCOMMAND | MF_GRAYED, SC_MAXIMIZE, (LPCSTR)"&Maksymalizuj");

	ACCEL SysMenuShort;
	SysMenuShort.fVirt = FALT;
	SysMenuShort.key = VK_SPACE;
	SysMenuShort.cmd = ID_SYSTEM_MENU;


    // Main message loop:
    while (GetMessage(&msg, nullptr, 0, 0))
    {
        if (!TranslateAccelerator(msg.hwnd, hAccelTable, &msg))
        {
            TranslateMessage(&msg);
            DispatchMessage(&msg);
        }
    }

    return (int) msg.wParam;
}



ATOM MyRegisterClass(HINSTANCE hInstance)
{
	WNDCLASSEXW wcex;
	wcex.cbSize = sizeof(WNDCLASSEX);

	wcex.style = CS_HREDRAW | CS_VREDRAW;
	wcex.lpfnWndProc = WndProc;
	wcex.cbClsExtra = 0;
	wcex.cbWndExtra = 0;
	wcex.hInstance = hInstance;
	wcex.hIcon = LoadIcon(hInstance, MAKEINTRESOURCE(IDI_ICON1));
	wcex.hCursor = LoadCursor(nullptr, IDC_ARROW);
	wcex.hbrBackground = (HBRUSH)CreateSolidBrush(RGB(239, 239, 245));
	wcex.lpszMenuName = MAKEINTRESOURCEW(IDC_BEJEWELED);
	wcex.lpszClassName = szWindowClass;
	wcex.hIconSm = LoadIcon(wcex.hInstance, MAKEINTRESOURCE(IDI_ICON1));

	return RegisterClassExW(&wcex);
}

ATOM MyRegisterTileClass(HINSTANCE hInstance)
{
	WNDCLASSEXW wcex;
	wcex.cbSize = sizeof(WNDCLASSEX);

	wcex.style = CS_HREDRAW | CS_VREDRAW;
	wcex.lpfnWndProc = TileWndProc;
	wcex.cbClsExtra = 0;
	wcex.cbWndExtra = 0;
	wcex.hInstance = hInstance;
	wcex.hIcon = LoadIcon(hInstance, MAKEINTRESOURCE(IDI_BEJEWELED));
	wcex.hCursor = LoadCursor(nullptr, IDC_ARROW);
	wcex.hbrBackground = (HBRUSH)CreateSolidBrush(RGB(41, 41, 61));
	wcex.lpszMenuName = MAKEINTRESOURCEW(IDC_BEJEWELED);
	wcex.lpszClassName = L"TileClass";
	wcex.hIconSm = LoadIcon(wcex.hInstance, MAKEINTRESOURCE(IDI_SMALL));

	return RegisterClassExW(&wcex);
}

ATOM MyRegisterParticleClass(HINSTANCE hInstance)
{
	WNDCLASSEXW wcex;
	wcex.cbSize = sizeof(WNDCLASSEX);

	wcex.style = CS_HREDRAW | CS_VREDRAW;
	wcex.lpfnWndProc = ParticleWndProc;
	wcex.cbClsExtra = 0;
	wcex.cbWndExtra = 0;
	wcex.hInstance = hInstance;
	wcex.hIcon = LoadIcon(hInstance, MAKEINTRESOURCE(IDI_BEJEWELED));
	wcex.hCursor = LoadCursor(nullptr, IDC_ARROW);
	wcex.hbrBackground = (HBRUSH)CreateSolidBrush(0x000000);
	wcex.lpszMenuName = NULL;
	wcex.lpszClassName = L"ParticleClass";
	wcex.hIconSm = LoadIcon(wcex.hInstance, MAKEINTRESOURCE(IDI_SMALL));

	return RegisterClassExW(&wcex);
}

//
//   FUNCTION: InitInstance(HINSTANCE, int) PURPOSE: Saves instance handle and creates main window

BOOL InitInstance(HINSTANCE hInstance, int nCmdShow)
{
	hInst = hInstance; // Store instance handle in our global variable

	//Main window
	hWndMain = CreateWindowW(szWindowClass, szTitle, WS_CAPTION | WS_SYSMENU | WS_MINIMIZEBOX | WS_CLIPCHILDREN,
		startX, startY, WIDTH, HEIGHT, nullptr, nullptr, hInstance, nullptr);
	ShowWindow(hWndMain, nCmdShow);
	UpdateWindow(hWndMain);

	//Particle Window
	hwndParticle = CreateWindowEx(WS_EX_TOPMOST | WS_EX_LAYERED | WS_EX_NOACTIVATE, L"ParticleClass", L"Particle", WS_POPUP | WS_VISIBLE,
		0, 0, ScreenX, ScreenY, hWndMain, SysMenu, hInstance, nullptr);
	SetLayeredWindowAttributes(hwndParticle, 0x000000, 0, LWA_COLORKEY);
	ShowWindow(hwndParticle, nCmdShow);
	UpdateWindow(hwndParticle);

	if (!hWndMain)
	{
		return FALSE;
	}

	for (int i = 0; i < MAX_BOARD_SIZE; i++)
	{
		for (int j = 0; j < MAX_BOARD_SIZE; j++)
		{
			Tiles[i][j].ChildWnd = CreateWindow(L"TileClass", L"Tile", WS_CHILDWINDOW | WS_VISIBLE,
				-tile_size, -tile_size, Sizes[0], Sizes[0], hWndMain, NULL, hInstance, NULL);
			SetWindowPos(Tiles[i][j].ChildWnd, HWND_TOP, 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE | SWP_SHOWWINDOW);
		}
	}
	AddTiles();

	return TRUE;
}


//
LRESULT CALLBACK WndProc(HWND hWnd, UINT message, WPARAM wParam, LPARAM lParam)
{
	RECT rc;
	POINT pt;
	COLORREF backgruond = RGB(239, 239, 245);
	static int count = 0;
	static int x = -1;
	static int y = -1;

	switch (message)
	{
	case WM_COMMAND:
	{
		int wmId = LOWORD(wParam);
		// Parse the menu selections:
		switch (wmId)
		{
		case IDM_ABOUT:
			DialogBox(hInst, MAKEINTRESOURCE(IDD_ABOUTBOX), hWnd, About);
			break;
		case ID_BOARDSIZE_SMALL:
			if (tile_number == Numbers[0]) break;
			KillTimer(hWnd, TIMER_NEWGAME);
			KillTimer(hWnd, TIMER_FALLING);
			ChangeBoardSize(0);
			break;
		case ID_BOARDSIZE_MEDIUM:
			if (tile_number == Numbers[1]) break;
			KillTimer(hWnd, TIMER_NEWGAME);
			KillTimer(hWnd, TIMER_FALLING);
			ChangeBoardSize(1);
			break;
		case ID_BOARDSIZE_BIG:
			if (tile_number == Numbers[2]) break;
			KillTimer(hWnd, TIMER_NEWGAME);
			KillTimer(hWnd, TIMER_FALLING);
			ChangeBoardSize(2);
			break;

		case ID_SYSTEM_MENU:   //Displaying system menu when Alt+Space is clicked
			SetForegroundWindow(hWnd);
			TrackPopupMenu(SysMenu, TPM_TOPALIGN | TPM_LEFTALIGN, 0, 0, 0, hWnd, NULL);
			break;

		case ID_NEWGAME:
			SetTimer(hWnd, TIMER_NEWGAME, NEWGAME_INTERVAL, NULL);  //coloring
			SetTimer(hWnd, TIMER_FALLING, FALLING_INTERVAL, NULL);   //falling tiles
			start = true;
			newgame = true;
			blockScreen = true;
			ColorTiles();
			break;

		case ID_HELP_DEBUG:
			if (start) break;
			if (debug) CheckMenuItem(hMenu, ID_HELP_DEBUG, MF_UNCHECKED);
			else CheckMenuItem(hMenu, ID_HELP_DEBUG, MF_CHECKED);
			debug = !debug;
			InvalidateRect(hwndParticle, NULL, TRUE);
			UpdateWindow(hwndParticle);
			break;

		case IDM_EXIT:
			DestroyWindow(hWnd);
			break;
		default:
			return DefWindowProc(hWnd, message, wParam, lParam);
		}
	}
	break;
	case WM_PAINT:
	{
		PAINTSTRUCT ps;
		HDC hdc = BeginPaint(hWnd, &ps);
		RECT rc;
		if (blockScreen || newgame) backgruond = RGB(0, 0, 51);  //changing background to black while tiles are falling
		else { backgruond = RGB(239, 239, 245); }

		HBRUSH hbrush;        //current brush  
		HBRUSH oldBrush;	 //default brush
		hbrush = CreateSolidBrush(backgruond);
		oldBrush = (HBRUSH)SelectObject(hdc, hbrush);
		GetClientRect(hWnd, &rc);
		FillRect(hdc, &rc, hbrush);

		SelectObject(hdc, oldBrush);
		DeleteObject(hbrush);
		EndPaint(hWnd, &ps);
	}
	break;

	case WM_ERASEBKGND:
		return 1;

	case WM_CREATE:
		_stprintf_s(buf, bufSize, _T("Bejeweled Game"));
		SetWindowText(hWnd, buf);
		break;

	case WM_TIMER:
		if (wParam == TIMER_NEWGAME) {
			if (start) {
				start = false;
				break;
			}
			if (newgame) {
				EnableMenuItem(hMenu, ID_NEWGAME, MF_BYCOMMAND | MF_GRAYED);
				EnableMenuItem(hMenu, ID_BOARDSIZE_SMALL, MF_BYCOMMAND | MF_GRAYED);
				EnableMenuItem(hMenu, ID_BOARDSIZE_MEDIUM, MF_BYCOMMAND | MF_GRAYED);
				EnableMenuItem(hMenu, ID_BOARDSIZE_BIG, MF_BYCOMMAND | MF_GRAYED);
				if (x < 0 && y < 0) { x = 0; y = 0; }
				InvalidateRect(hWnd, NULL, TRUE);
				InvalidateRect(Tiles[x][y].ChildWnd, NULL, TRUE);
				y++;
				if (y > tile_number - 1) {
					x++;
					y = 0;
				}
				if (y == tile_number - 1 && x == tile_number - 1) {
					InvalidateRect(Tiles[x][y].ChildWnd, NULL, TRUE);
					x = -1;
					y = -1;
					newgame = false;
					InvalidateRect(hWnd, NULL, TRUE);
					EnableMenuItem(hMenu, ID_NEWGAME, MF_BYCOMMAND | MF_ENABLED);
					EnableMenuItem(hMenu, ID_BOARDSIZE_SMALL, MF_BYCOMMAND | MF_ENABLED);
					EnableMenuItem(hMenu, ID_BOARDSIZE_MEDIUM, MF_BYCOMMAND | MF_ENABLED);
					EnableMenuItem(hMenu, ID_BOARDSIZE_BIG, MF_BYCOMMAND | MF_ENABLED);
					KillTimer(hWnd, TIMER_NEWGAME);
				}
			}
		}
		if (wParam == TIMER_FALLING) {
			if (!newgame) {
				if (pause) {
					pause = false;
					blockScreen = false;
					InvalidateRect(hWnd, NULL, TRUE);
				}
				if (!falling_tiles) {
					if (ThreeInRow() || ThreeInColumn()) {
						falling_tiles = true;
						blockScreen = true;
						InvalidateRect(hWnd, NULL, TRUE);
						CreateParticles();
						
						break;
					}
				}
				if (blockScreen) {
					for (int i = 0; i < tile_number; i++) {
						int j = 0;
						for (; j < tile_number && !TilesToDestroy[j][i]; j++);   //find the first tile which was 'destroyed'
						if (j == tile_number) continue;
						VerticalFall(j,i);  //move one tile down in one clock tick in each column where match was found
					}
					if (!FindMove()) {
						falling_tiles = false;
						pause = true;
					}
				}
				
			}
		}
		break;

	case WM_NCRBUTTONDOWN:  //displaying system menu
		GetCursorPos(&pt);
		SetForegroundWindow(hWnd);
		TrackPopupMenu(SysMenu, TPM_TOPALIGN | TPM_LEFTALIGN, pt.x, pt.y, 0, hWnd, NULL);
		break;

	case WM_DESTROY:
		DestroyWindow(hWnd);
		for (int i = 0; i < MAX_BOARD_SIZE; i++) {
			for (int j = 0; j < MAX_BOARD_SIZE; j++) {
				DestroyWindow(Tiles[i][j].ChildWnd);
			}
		}
		KillTimer(hWnd, TIMER_NEWGAME);
		KillTimer(hWnd, TIMER_FALLING);
        PostQuitMessage(0);
        break;
    default:
        return DefWindowProc(hWnd, message, wParam, lParam);
    }
    return 0;
}



LRESULT CALLBACK TileWndProc(HWND hWnd, UINT message, WPARAM wParam, LPARAM lParam)
{
	COLORREF bkcolor = RGB(41, 41, 61);
	BOOL MouseTracking = FALSE;

	static bool selected = false;  //currently selected window
	bool cross = false;           //bool to store information about current window cross pattern
	static bool changed = false;  //indicates if window is currently resized

	switch (message)
	{
	case WM_COMMAND:
	{
		int wmId = LOWORD(wParam);
		// Parse the menu selections:
		switch (wmId)
		{
		case IDM_ABOUT:
			DialogBox(hInst, MAKEINTRESOURCE(IDD_ABOUTBOX), hWnd, About);
			break;
		case IDM_EXIT:
			DestroyWindow(hWnd);
			break;
		default:
			return DefWindowProc(hWnd, message, wParam, lParam);
		}
	}
	break;
	case WM_PAINT:
	{
		PAINTSTRUCT ps;
		HDC hdc = BeginPaint(hWnd, &ps);
		RECT rc;
		if (start) bkcolor = RGB(41, 41, 61);  //at start painting with grey
		else { bkcolor = FindColor(hWnd); }    //after only with color assigned in new game

		cross = FindCrossPattern(hWnd);
		HBRUSH hbrush;              // current brush  
		HBRUSH oldBrush;
		hbrush = CreateSolidBrush(bkcolor);
		oldBrush = (HBRUSH)SelectObject(hdc, hbrush);
		GetClientRect(hWnd, &rc);
		FillRect(hdc, &rc, hbrush);

		if (cross) {
			hbrush = CreateHatchBrush(HS_CROSS, bkcolor);
			oldBrush = (HBRUSH)SelectObject(hdc, hbrush);
			GetClientRect(hWnd, &rc);
			FillRect(hdc, &rc, hbrush);
		}

		SelectObject(hdc, oldBrush);
		DeleteObject(hbrush);
		DeleteObject(oldBrush);

		if (selected && selectedWindowHwnd == hWnd && !start)
		{
			HPEN hPen = CreatePen(PS_SOLID, 6, RGB(0, 0, 0));
			HPEN oldpen = (HPEN)SelectObject(hdc, hPen);
			//SelectObject(hdc, hPen);
			MoveToEx(hdc, rc.left + 2, rc.top + 2, NULL);
			LineTo(hdc, rc.right - 2, rc.top + 2);
			LineTo(hdc, rc.right - 2, rc.bottom - 2);
			LineTo(hdc, rc.left + 2, rc.bottom - 2);
			LineTo(hdc, rc.left + 2, rc.top + 2);
			SelectObject(hdc, oldpen);
			DeleteObject(hPen);
		}
		else if (!selected && selectedWindowHwnd == hWnd && !start)
		{
			HPEN hPen1 = CreatePen(PS_SOLID, 6, bkcolor);;
			HPEN holdpen = (HPEN)SelectObject(hdc, hPen1);
			MoveToEx(hdc, rc.left + 2, rc.top + 2, NULL);
			LineTo(hdc, rc.right - 2, rc.top + 2);
			LineTo(hdc, rc.right - 2, rc.bottom - 2);
			LineTo(hdc, rc.left + 2, rc.bottom - 2);
			LineTo(hdc, rc.left + 2, rc.top + 2);
			SelectObject(hdc, holdpen);
			DeleteObject(hPen1);
		}
		
		EndPaint(hWnd, &ps);
	}
	break;
	case WM_ERASEBKGND:
		return 1;

	case WM_CREATE:
	{
		bkcolor = RGB(41, 41, 61);
	}
	break;

	case WM_LBUTTONDOWN:
		if (selectedWindowHwnd == NULL && !start && !newgame && !selected)  //if there is no selected window, select current window
		{
			selected = true;
			selectedWindowHwnd = hWnd;
			InvalidateRect(hWnd, NULL, TRUE);
		}
		else if (hWnd == selectedWindowHwnd && selected)   //if this window is selected - deselect
		{
			selected = false;
			selectedWindowHwnd = NULL;
			InvalidateRect(hWnd, NULL, TRUE);
		}
		else if (selected && hWnd != selectedWindowHwnd)  //checking if user's move is correct
		{
			windowToSwap = hWnd;
			bool test = IsMoveCorrect(selectedWindowHwnd, windowToSwap);
			if (!test) {
				test = IsMoveCorrect(windowToSwap, selectedWindowHwnd);
			}
			if (test) {
				int i, j, a, b, x, y;
				for (i = 0; i < tile_number; i++) {
					for (j = 0; j < tile_number; j++)
					{
						if (selectedWindowHwnd == Tiles[i][j].ChildWnd) {
							x = i; y = j;
						}
						if (windowToSwap == Tiles[i][j].ChildWnd) {
							a = i; b = j;
						}
					}
				}
				selected = false;
				selectedWindowHwnd = NULL;
				COLORREF tmp = Tiles[x][y].BkGndColor;
				Tiles[x][y].BkGndColor = Tiles[a][b].BkGndColor;
				Tiles[a][b].BkGndColor = tmp;
				InvalidateRect(Tiles[x][y].ChildWnd, NULL, TRUE);
				InvalidateRect(Tiles[a][b].ChildWnd, NULL, TRUE);

			}
			HWND temp = selectedWindowHwnd;   //deselecting current window regardless of correctness of user's move
			selected = false;
			selectedWindowHwnd = NULL;
			InvalidateRect(temp, NULL, TRUE);
			UpdateWindow(temp);
		}
		break;

	case WM_MOUSEMOVE:
		if (!changed && !newgame)
		{
			changed = true;
			TRACKMOUSEEVENT tme;
			tme.cbSize = sizeof(TRACKMOUSEEVENT);
			tme.dwFlags = TME_LEAVE;
			tme.hwndTrack = hWnd;
			MouseTracking = TrackMouseEvent(&tme);

			RECT rc;
			GetClientRect(hWnd, &rc);
			updateTile(hWnd, rc, 4 - (rc.right - rc.left - tile_size) / 2);
		}
		break;

	case WM_MOUSELEAVE:
	{
		changed = false;
		TRACKMOUSEEVENT tme;
		tme.cbSize = sizeof(TRACKMOUSEEVENT);
		tme.dwFlags = TME_CANCEL | TME_LEAVE;
		tme.hwndTrack = hWnd;
		MouseTracking = TrackMouseEvent(&tme);
		SetTimer(hWnd, TIMER_MOUSE, MOUSE_INTERVAL, NULL);
	}
	break;

	case WM_TIMER:
		if (wParam == TIMER_MOUSE) {
			RECT rect;
			GetClientRect(hWnd, &rect);
			if (rect.right - rect.left == tile_size)
			{
				KillTimer(hWnd, TIMER_MOUSE);
				break;
			}
			updateTile(hWnd, rect, -1);
			InvalidateRect(GetParent(hWnd), NULL, TRUE);
		}
		break;

	case WM_DESTROY:
		KillTimer(hWnd, TIMER_MOUSE);
		PostQuitMessage(0);
		changed = false;
		break;
	default:
		return DefWindowProc(hWnd, message, wParam, lParam);
	}
	return 0;
}


LRESULT CALLBACK ParticleWndProc(HWND hWnd, UINT message, WPARAM wParam, LPARAM lParam)
{
	COLORREF pixelcolor = HOLLOW_BRUSH;

	switch (message)
	{
	case WM_COMMAND:
	{
		int wmId = LOWORD(wParam);
		// Parse the menu selections:
		switch (wmId)
		{
		case IDM_ABOUT:
			DialogBox(hInst, MAKEINTRESOURCE(IDD_ABOUTBOX), hWnd, About);
			break;
		case IDM_EXIT:
			DestroyWindow(hWnd);
			break;
		default:
			return DefWindowProc(hWnd, message, wParam, lParam);
		}
	}
	break;

	case WM_PAINT:
	{
		PAINTSTRUCT ps;
		RECT rc;
		HBRUSH brush, oldbrush;
		HBITMAP bmp, oldbmp;
		HDC hdc = BeginPaint(hWnd, &ps);
		HDC HCDC = CreateCompatibleDC(hdc);

		bmp = CreateCompatibleBitmap(hdc, ScreenX, ScreenY);
		oldbmp = (HBITMAP)SelectObject(HCDC, bmp);
		brush = (HBRUSH)GetStockObject(DC_BRUSH);
		oldbrush = (HBRUSH)SelectObject(HCDC, brush);

		GetWindowRect(hWnd, &rc);
		SetDCBrushColor(HCDC, 0x000000);
		FillRect(HCDC, &rc, (HBRUSH)GetCurrentObject(HCDC, OBJ_BRUSH));

		if (debug) {
			HFONT font = CreateFont(100, // Height
				0, // Width
				0, // Escapement
				0, // Orientation
				FW_BOLD, // Weight
				false, // Italic
				FALSE, // Underline
				0, // StrikeOut
				EASTEUROPE_CHARSET, // CharSet
				OUT_DEFAULT_PRECIS, // OutPrecision
				CLIP_DEFAULT_PRECIS, // ClipPrecision
				ANTIALIASED_QUALITY, // Quality
				DEFAULT_PITCH | FF_SWISS, // PitchAndFamily
				_T(" Verdana ")); // Facename
			HFONT oldFont = (HFONT)SelectObject(HCDC, font);
			std::wstring title = L"Particles: ";
			title += std::to_wstring(particles.size());
			SetBkColor(HCDC, 0x000000);
			SetTextColor(HCDC, RGB(102, 0, 102));
			DrawText(HCDC, title.c_str(), (int)_tcslen(title.c_str()), &rc, DT_CENTER | DT_SINGLELINE);
			SelectObject(HCDC, oldFont);
			DeleteObject(font);
		}

		for (auto& p : particles)
		{
			p.position.x += 10 * p.direction_x;
			p.position.y += 10 * p.direction_y;
			SetDCBrushColor(HCDC, p.colour);
			Rectangle(HCDC, p.position.x, p.position.y, p.position.x + particle_size, p.position.y + particle_size);
		}
		BitBlt(hdc, 0, 0, ScreenX, ScreenY, HCDC, 0, 0, SRCCOPY);
		brush = (HBRUSH)SelectObject(HCDC, oldbrush);
		bmp = (HBITMAP)SelectObject(HCDC, oldbmp);
		DeleteObject(brush);
		DeleteObject(bmp);
		DeleteDC(HCDC);

		EndPaint(hWnd, &ps);
	}
	break;
	case WM_CREATE:
		SetTimer(hWnd, TIMER_PARTICLES, PARTICLES_INTERVAL, NULL);
		break;

	case WM_TIMER:
	{
		if (wParam == TIMER_PARTICLES) //Redraws the particles at each tick of the clock 
		{ 
			if (particles.size() == 0) break;
			RemoveParticles();                         //remove particles which reached end of screen
			InvalidateRect(hWnd, NULL, FALSE);
			UpdateWindow(hWnd);
		}
	}
	break;

	case WM_DESTROY:
		KillTimer(hWnd, TIMER_PARTICLES);
		DestroyWindow(hWnd);
		PostQuitMessage(0);
		break;
	default:
		return DefWindowProc(hWnd, message, wParam, lParam);
	}
	return 0;
}




// Message handler for about box.
INT_PTR CALLBACK About(HWND hDlg, UINT message, WPARAM wParam, LPARAM lParam)
{
    UNREFERENCED_PARAMETER(lParam);
    switch (message)
    {
    case WM_INITDIALOG:
        return (INT_PTR)TRUE;

    case WM_COMMAND:
        if (LOWORD(wParam) == IDOK || LOWORD(wParam) == IDCANCEL)
        {
            EndDialog(hDlg, LOWORD(wParam));
            return (INT_PTR)TRUE;
        }
        break;
    }
    return (INT_PTR)FALSE;
}


void AddTiles()
{
	srand(time(NULL));
	int x = MARGIN;
	int y = MARGIN;
	int i, j, r;
	startX = (ScreenX - WIDTH) / 2;
	startY = (ScreenY - HEIGHT) / 2;

	for (i = 0; i < tile_number; i++)
	{
		for (j = 0; j < tile_number; j++)
		{
			MoveWindow(Tiles[i][j].ChildWnd, x, y, tile_size, tile_size, TRUE);
			ShowWindow(Tiles[i][j].ChildWnd, 10);
			UpdateWindow(Tiles[i][j].ChildWnd);

			r = rand() % 6;
			Tiles[i][j].BkGndColor = Colors[r];
			Tiles[i][j].crosspattern = false;

			x += tile_size + MARGIN;
		}
		x = MARGIN;
		y += tile_size + MARGIN;
	}
	for (int k = i; k < MAX_BOARD_SIZE; k++) {
		for (int l = 0; l < MAX_BOARD_SIZE; l++) {
			MoveWindow(Tiles[k][l].ChildWnd, -tile_size, -tile_size, tile_size, tile_size, TRUE);
			MoveWindow(Tiles[l][k].ChildWnd, -tile_size, -tile_size, tile_size, tile_size, TRUE);

			ShowWindow(Tiles[k][l].ChildWnd, 10);
			UpdateWindow(Tiles[k][l].ChildWnd);

			ShowWindow(Tiles[l][k].ChildWnd, 10);
			UpdateWindow(Tiles[l][k].ChildWnd);
		}
	}

	MoveWindow(hWndMain, startX, startY, WIDTH, HEIGHT, TRUE);
	UpdateWindow(hWndMain);
	//DestroyWindow(hWnd);
}

void ColorTiles()
{
	int i, j;
	for (i = 0; i < tile_number; i++)
	{
		for (j = 0; j < tile_number; j++)
		{
			InvalidateRect(Tiles[i][j].ChildWnd, NULL, TRUE);
		}
	}
}

void updateTile(HWND hWnd, RECT rc, int n)
{
	POINT tmp{ rc.left, rc.top };
	MapWindowPoints(hWnd, GetParent(hWnd), (LPPOINT)&tmp, 1);
	InflateRect(&rc, n, n);
	//if (windowHeight > tile_size + 8 || windowWidth > tile_size + 8) return;
	MoveWindow(hWnd, tmp.x - n, tmp.y - n, rc.right - rc.left, rc.bottom - rc.top, TRUE);
}

COLORREF FindColor(HWND window)
{
	int i, j;
	for (i = 0; i < tile_number; i++) {
		for (j = 0; j < tile_number; j++)
		{
			if (window == Tiles[i][j].ChildWnd)
				return Tiles[i][j].BkGndColor;
		}
	}
	return NULL;
}

bool FindCrossPattern(HWND window)
{
	int i, j;
	for (i = 0; i < tile_number; i++) {
		for (j = 0; j < tile_number; j++)
		{
			if (window == Tiles[i][j].ChildWnd)
				return Tiles[i][j].crosspattern;
		}
	}
	return false;
}

void ChangeBoardSize(int newSize)
{
	if (tile_number == Numbers[0]) CheckMenuItem(hMenu, ID_BOARDSIZE_SMALL, MF_BYCOMMAND | MF_UNCHECKED);
	if (tile_number == Numbers[1]) CheckMenuItem(hMenu, ID_BOARDSIZE_MEDIUM, MF_BYCOMMAND | MF_UNCHECKED);
	if (tile_number == Numbers[2]) CheckMenuItem(hMenu, ID_BOARDSIZE_BIG, MF_BYCOMMAND | MF_UNCHECKED);
	CheckMenuItem(hMenu, ID_BOARDSIZE_SMALL + newSize, MF_BYCOMMAND | MF_CHECKED);
	
	tile_number = Numbers[newSize];
	tile_size = Sizes[newSize];
	WIDTH = (Numbers[newSize] + 1)*MARGIN + Sizes[newSize] * Numbers[newSize] + 15;
	HEIGHT = (Numbers[newSize] + 1)*MARGIN + Sizes[newSize] * Numbers[newSize] + 60;
	particle_size = tile_size / MARGIN;
	start = true;
	newgame = false;
	AddTiles();
}



void DestroyTiles(int ile, int row, int col, bool vertical) 
{
	//int r;
	if (vertical) {
		for (int a = row; a < row + ile; a++) {
			TilesToDestroy[a][col] = true;
			Tiles[a][col].crosspattern = true;
			InvalidateRect(Tiles[a][col].ChildWnd, NULL, TRUE);   //repainting with cross pattern
		}
	}
	else {
		for (int b = col; b < col + ile; b++) {
			TilesToDestroy[row][b] = true;
			Tiles[row][b].crosspattern = true;
			InvalidateRect(Tiles[row][b].ChildWnd, NULL, TRUE);  
		}
	}
}

bool ThreeInRow()
{
	int i, j;
	bool found = false;
	int counter = 1;

	for (i = 0; i < tile_number; i++)  //searching for horizontal match
	{
		for (j = 0; j < tile_number; j++)
		{

			if (j + 1 == tile_number) break;
			if (Tiles[i][j].BkGndColor != Tiles[i][j + 1].BkGndColor) {
				continue;
			}
			else {
				if (j + 2 == tile_number) { break; counter = 1; }
				counter = 2;
				int k = j + 2;
				while (Tiles[i][j].BkGndColor == Tiles[i][k].BkGndColor && k < tile_number) {
					counter++;
					k++;
				}

				if (counter > 2) {
					bool test = ThreeInColumn();
					DestroyTiles(counter, i, j, false);
					int k = 0;
					while (k < counter) {
						TilesToDestroy[i][j+k] = true;
						k++;
					}
					found = true;
					j += counter;
				}
				counter = 1;
			}
		}

	}
	return found;
}

bool ThreeInColumn()
{
	int i, j;
	int counter = 1;
	bool found = false;
	
	for (i = 0; i < tile_number; i++)  //searching for vertical match
	{
		for (j = 0; j < tile_number; j++)
		{
			if (i + 1 == tile_number - 1) break;
			else if (Tiles[i][j].BkGndColor != Tiles[i + 1][j].BkGndColor) {
				continue;
			}
			else {
				if (i + 2 == tile_number) { break; counter = 1; }
				counter = 2;
				int k = i + 2;
				while (Tiles[i][j].BkGndColor == Tiles[k][j].BkGndColor && k < tile_number) {
					counter++;
					k++;
				}

				if (counter > 2) {
					DestroyTiles(counter, i, j, true);
					int k = 0;
					while (k < counter) {
						TilesToDestroy[i + k][j] = true;
						k++;
					}
					found = true;
				}
				counter = 1;
			}
		}
	}
	return found;
}

int HorizontalFall(int ile, int row, int col)
{
	int j;
	for (j = row; j >= 0; j--) {
		for (int i = col; i < col + ile; i++) {
			Tiles[j][i].crosspattern = false;
			TilesToDestroy[i][j] = false;
			if (j == 0) {
				int r = rand() % 6;
				Tiles[j][i].BkGndColor = Colors[r];
				InvalidateRect(Tiles[j][i].ChildWnd, NULL, TRUE);
			}
			else {
				Tiles[j][i].BkGndColor = Tiles[j - 1][i].BkGndColor;
				InvalidateRect(Tiles[j][i].ChildWnd, NULL, TRUE);
			}
		}
	}
	return j - 1;
}

void VerticalFall(int row, int col)
{
	Tiles[row][col].crosspattern = false;
	TilesToDestroy[row][col] = false;
	if (row == 0) {
		int r = rand() % 6;
		Tiles[row][col].BkGndColor = Colors[r];
		InvalidateRect(Tiles[row][col].ChildWnd, NULL, TRUE);
	}
	else {
		for (int i = row; i >= 0; i--) {
			if (i == 0) {
				Tiles[i][col].BkGndColor = Colors[rand() % 6];
				InvalidateRect(Tiles[i][col].ChildWnd, NULL, TRUE);
			}
			else {
				Tiles[i][col].BkGndColor = Tiles[i - 1][col].BkGndColor;
				InvalidateRect(Tiles[i][col].ChildWnd, NULL, TRUE);
			}
		}
	}
}

bool FindMove()
{
	bool result = false;
	for (int i = 0; i < tile_number; i++)
	{
		int j = 0;
		for (; j < tile_number && !TilesToDestroy[j][i]; j++);
		if (j == tile_number) continue;  //if there was none continue
		return true;
	}
	return false;
}

bool IsMoveCorrect(HWND window1_selected, HWND window2_Toselect)
{
	int i, j;
	for (i = 0; i < tile_number; i++) {
		for (j = 0; j < tile_number; j++)
		{
			if (window1_selected == Tiles[i][j].ChildWnd)
				goto end;
		}
	}
end:
	if (j - 1 >= 0) {  //move to left
		if (j - 2 > 0 && Tiles[i][j - 2].BkGndColor == Tiles[i][j].BkGndColor &&  Tiles[i][j - 3].BkGndColor == Tiles[i][j].BkGndColor && Tiles[i][j - 1].ChildWnd == window2_Toselect) //**_*
			return true;
		if (i - 1 >= 0 && i + 1 <= tile_number - 1 && Tiles[i][j].BkGndColor == Tiles[i - 1][j - 1].BkGndColor && Tiles[i][j].BkGndColor == Tiles[i + 1][j - 1].BkGndColor && Tiles[i][j - 1].ChildWnd == window2_Toselect) //*_*
			return true;
		if (i + 2 <= tile_number - 1 && Tiles[i][j].BkGndColor == Tiles[i + 1][j - 1].BkGndColor && Tiles[i][j].BkGndColor == Tiles[i + 2][j - 1].BkGndColor && Tiles[i][j - 1].ChildWnd == window2_Toselect)
			return true;
		if (i - 2 >= 0 && Tiles[i][j].BkGndColor == Tiles[i - 1][j - 1].BkGndColor && Tiles[i][j].BkGndColor == Tiles[i - 2][j - 1].BkGndColor && Tiles[i][j - 1].ChildWnd == window2_Toselect)
			return true;
	}

	if (j + 1 <= tile_number - 1) {  //move to right
		if (j + 2 < tile_number - 1 && Tiles[i][j + 2].BkGndColor == Tiles[i][j].BkGndColor &&  Tiles[i][j + 3].BkGndColor == Tiles[i][j].BkGndColor && Tiles[i][j + 1].ChildWnd == window2_Toselect) //*_**
			return true;
		if (i - 1 >= 0 && i + 1 <= tile_number - 1 && Tiles[i][j].BkGndColor == Tiles[i - 1][j + 1].BkGndColor && Tiles[i][j].BkGndColor == Tiles[i + 1][j + 1].BkGndColor && Tiles[i][j + 1].ChildWnd == window2_Toselect) //*_*
			return true;
		if (i + 2 <= tile_number - 1 && Tiles[i][j].BkGndColor == Tiles[i + 1][j + 1].BkGndColor && Tiles[i][j].BkGndColor == Tiles[i + 2][j + 1].BkGndColor && Tiles[i][j + 1].ChildWnd == window2_Toselect)
			return true;
		if (i - 2 >= 0 && Tiles[i][j].BkGndColor == Tiles[i - 1][j + 1].BkGndColor && Tiles[i][j].BkGndColor == Tiles[i - 2][j + 1].BkGndColor && Tiles[i][j + 1].ChildWnd == window2_Toselect)
			return true;
	}

	if (i - 1 >= 0) {  //move up
		if (i - 2 > 0 && Tiles[i][j].BkGndColor == Tiles[i - 2][j].BkGndColor && Tiles[i][j].BkGndColor == Tiles[i - 3][j].BkGndColor && Tiles[i - 1][j].ChildWnd == window2_Toselect)
			return true;
		if (j - 1 >= 0 && j + 1 <= tile_number - 1 && Tiles[i][j].BkGndColor == Tiles[i - 1][j - 1].BkGndColor && Tiles[i][j].BkGndColor == Tiles[i - 1][j + 1].BkGndColor && Tiles[i - 1][j].ChildWnd == window2_Toselect)  //*_*
			return true;
		if (j - 2 >= 0 && Tiles[i][j].BkGndColor == Tiles[i - 1][j - 2].BkGndColor && Tiles[i][j].BkGndColor == Tiles[i - 1][j - 1].BkGndColor && Tiles[i - 1][j].ChildWnd == window2_Toselect)  //**_
			return true;
		if (j + 2 <= tile_number - 1 && Tiles[i][j].BkGndColor == Tiles[i - 1][j + 2].BkGndColor && Tiles[i][j].BkGndColor == Tiles[i - 1][j + 1].BkGndColor && Tiles[i - 1][j].ChildWnd == window2_Toselect)  //_**
			return true;
	}

	if (i + 1 <= tile_number - 1) {  //move down
		if (i + 2 < tile_number - 1 && Tiles[i][j].BkGndColor == Tiles[i + 2][j].BkGndColor && Tiles[i][j].BkGndColor == Tiles[i + 3][j].BkGndColor && Tiles[i + 1][j].ChildWnd == window2_Toselect)
			return true;
		if (j - 1 >= 0 && j + 1 <= tile_number - 1 && Tiles[i][j].BkGndColor == Tiles[i + 1][j - 1].BkGndColor && Tiles[i][j].BkGndColor == Tiles[i + 1][j + 1].BkGndColor && Tiles[i + 1][j].ChildWnd == window2_Toselect)  //*_*
			return true;
		if (j - 2 >= 0 && Tiles[i][j].BkGndColor == Tiles[i + 1][j - 2].BkGndColor && Tiles[i][j].BkGndColor == Tiles[i + 1][j - 1].BkGndColor && Tiles[i + 1][j].ChildWnd == window2_Toselect)  //**_
			return true;
		if (j + 2 <= tile_number - 1 && Tiles[i][j].BkGndColor == Tiles[i + 1][j + 2].BkGndColor && Tiles[i][j].BkGndColor == Tiles[i + 1][j + 1].BkGndColor && Tiles[i + 1][j].ChildWnd == window2_Toselect)  //_**
			return true;
	}

	return false;
}



void CreateParticles()
{
	srand(time(NULL));
	bool result = false;
	for (int i = 0; i < tile_number; i++)
	{
		for (int j = 0; j < tile_number; j++)
		{
			if (TilesToDestroy[i][j])
			{
				int x = (j + 1)*MARGIN + j * tile_size;
				int y = (i + 1)*MARGIN + i * tile_size;
				POINT tmp = { x, y };
				MapWindowPoints(hWndMain, hwndParticle, (LPPOINT)&tmp, 1);
				for (int a = 0; a < MARGIN; a++)
				{
					for (int b = 0; b < MARGIN; b++) {
						double dirx = (rand() % ScreenX + 1) * pow(-1, rand() % 8);
						double diry = (rand() % ScreenY + 1) * pow(-1, rand() % 8);
						double length = pow(pow(dirx, 2) + pow(diry, 2), 0.5);
						dirx /= length;
						diry /= length;
						particles.push_back(Particle{ POINT{ tmp.x + b * particle_size, tmp.y + a * particle_size }, dirx, diry, Tiles[i][j].BkGndColor });
					}
				}
			}
		}
	}
}


void RemoveParticles()
{
	auto iterator = std::remove_if(particles.begin(), particles.end(), [&](Particle p) {
		return p.position.x > ScreenX || p.position.x < 0 || p.position.y > ScreenY || p.position.y < 0;
	});
	particles.resize(iterator - particles.begin());
}