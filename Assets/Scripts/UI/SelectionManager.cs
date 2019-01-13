using Scripts.Ship;

namespace Scripts.UI
{
    class SelectionManager
    {
        private static SelectionManager _instance;
        public static SelectionManager Instance
        {
            get { return _instance ?? (_instance = new SelectionManager()); }
        }

        private ShipAppearance _ship;

        public void SelectShip(ShipAppearance selectedShip)
        {
            if (_ship != null) {_ship.IsSelected = false;}
            _ship = selectedShip;
            _ship.IsSelected = true;
        }

        public ShipAppearance GetSelectedShip()
        {
            return _ship;
        }
    }
}
