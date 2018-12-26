namespace Assets.Scripts
{
    class SelectionManager
    {
        private static SelectionManager _instance;
        public static SelectionManager Instance
        {
            get
            {
                if (_instance == null) _instance = new SelectionManager();

                return _instance;
            }
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
