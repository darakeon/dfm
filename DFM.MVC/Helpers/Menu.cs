using System;

namespace DFM.MVC.Helpers
{
    public class Menu
    {
        public String ID { get; private set; }
        public String Text { get; private set; }
        public String Action { get; private set; }
        public String Controller { get; private set; }

        internal Menu(String id, String text, String action, String controller)
        {
            ID = id;
            Text = text;
            Action = action;
            Controller = controller;
        }
    }
}