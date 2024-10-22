namespace ANG24.Core.Entities.DataTypes
{
    public class MKZStateInfo
    {
        public FazeType FazeType { get; set; } = FazeType.ThreeFaze;

        private bool _doorLeft;
        private bool _doorRight;
        private bool _dangerousPotencial;
        private bool _ground;
        private bool _safeKey;
        private bool _stop;

        public bool MKZError
        {
            get
            {
                if (FazeType == FazeType.ThreeFaze)
                {
                    if (!Stop || !SafeKey || !DangerousPotencial || !Ground || !DoorLeft || !DoorRight) return true;
                    else return false;
                }
                else
                {
                    if (!Stop || !SafeKey || !DangerousPotencial || !DoorLeft || !DoorRight) return true;
                    else return false;
                }
            }
        }
        public bool DoorLeft
        {
            get => _doorLeft;
            set
            {
                _doorLeft = value;
                _ = MKZError;
            }
        }
        public bool DoorRight
        {
            get => _doorRight;
            set
            {
                _doorRight = value;
                _ = MKZError;
            }
        }
        public bool DangerousPotencial
        {
            get => _dangerousPotencial;
            set
            {
                _dangerousPotencial = value;
                _ = MKZError;
            }
        }
        public bool Ground
        {
            get => _ground;
            set
            {
                _ground = value;
                _ = MKZError;
            }
        }
        public bool SafeKey
        {
            get => _safeKey;
            set
            {
                _safeKey = value;
                _ = MKZError;
            }
        }
        public bool Stop
        {
            get => _stop;
            set
            {
                _stop = value;
                _ = MKZError;
            }
        }
        public MKZStateInfo(string InputData)
        {
            var i = int.Parse(InputData);
            if ((i & 0x01) != 0) Stop = true;
            if ((i & 0x02) != 0) SafeKey = true;
            if ((i & 0x04) != 0) DangerousPotencial = true;
            if ((i & 0x08) != 0) Ground = true;
            if ((i & 0x10) != 0) DoorLeft = true;
            if ((i & 0x20) != 0) DoorRight = true;
            //if (Stop || SafeKey || DangerousPotencial || Ground || DoorLeft || DoorRight) MKZError = true;
        }
        public MKZStateInfo()
        {
            //if (Stop || SafeKey || DangerousPotencial || Ground || DoorLeft || DoorRight) MKZError = true;
        }
    }
}
