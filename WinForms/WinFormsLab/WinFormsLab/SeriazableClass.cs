using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Runtime.Serialization;
using System.Windows.Forms;

namespace WinFormsLab
{
    [Serializable]
    class SerializableClass
    {
        public Image pictureBoxImage { get; set; }
        public List<ListViewItem> listViewItems { get; set; }
        public List<FurnitureClass> bitmapElements { get; set; }

        public SerializableClass(List<ListViewItem> list, Image picture, List<FurnitureClass> elem)
        {
            pictureBoxImage = picture;
            listViewItems = list;
            bitmapElements = elem;
        }
        //public void GetObjectData(SerializationInfo info, StreamingContext context)
        //{
        //    info.AddValue("pictureBoxImage", pictureBoxImage, typeof(Image));
        //    info.AddValue("listViewItems", listViewItems, typeof(ListView.ListViewItemCollection));
        //}

        //private void SetObjectData(SerializationInfo info, StreamingContext context)
        //{
        //    pictureBoxImage = (Image)info.GetValue("pictureBoxImage", typeof(Image));
        //    listViewItems = (List<ListViewItem>)info.GetValue("listViewItems", typeof(List<ListViewItem>));
        //}


    }
}
