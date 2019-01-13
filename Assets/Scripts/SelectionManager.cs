namespace Assets.Scripts
{
    class SelectionManager
    {
        private static SelectionManager _instance;
        public static SelectionManager Instance
        {
            get { return _instance ?? (_instance = new SelectionManager()); }
        }

        private OldShip _selectedOldShip;

        public void SelectShip(OldShip oldShip)
        {
            if (_selectedOldShip != null) {_selectedOldShip.Selected = false;}
            _selectedOldShip = oldShip;
            _selectedOldShip.Selected = true;
        }

        public OldShip GetSelectedShip()
        {
            return _selectedOldShip;
        }
    }
}
