namespace Assets.Scripts
{
    class SelectionManager
    {
        private static SelectionManager _instance;
        public static SelectionManager Instance
        {
            get { return _instance ?? (_instance = new SelectionManager()); }
        }


        private Ship _selectedShip;

        public void SelectShip(Ship ship)
        {
            if (_selectedShip != null) {_selectedShip.Selected = false;}
            _selectedShip = ship;
            _selectedShip.Selected = true;
        }

        public Ship GetSelectedShip()
        {
            return _selectedShip;
        }
    }
}
