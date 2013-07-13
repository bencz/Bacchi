namespace Bacchi.Kernel
{
    public class Version
    {
        private string _name;
        /** The name of the product. */
        public string Name
        {
            get { return _name; }
        }

        private int _major;
        /** The major version number (0 = 0.xx). */
        public int Major
        {
            get { return _major; }
        }

        private int _minor;
        /** The minor version number (0 = x.00). */
        public int Minor
        {
            get { return _minor; }
        }

        private int _micro;
        /** The micro version number, if any (may be zero). */
        public int Micro
        {
            get { return _micro; }
        }

        private int _build;
        /** The build number, if any (may be zero). */
        public int Build
        {
            get { return _build; }
        }

        private string _home;
        /** The home URL of the product, if any (may be \c null). */
        public string Home
        {
            get { return _home; }
        }

        private string _right;
        /** The copyright or copyleft clause: "Copyright (C)" or "Copyleft (-)". */
        public string Right
        {
            get { return _right; }
        }

        private string _owner;
        /** The legal entity that is publishing this product. */
        public string Owner
        {
            get { return _owner; }
        }

        private int _lower;
        /** The lower value of the copyright year range (XXXX-YYYY). */
        public int Lower
        {
            get { return _lower; }
        }

        private int _upper;
        /** The upper value of the copyright year range (XXXX-YYYY). */
        public int Upper
        {
            get { return _upper; }
        }

        private string _claim;
        /** The legal rights being claimed: "All rights reserved." or "No rights reserved." */
        public string Claim
        {
            get { return _claim; }
        }

        public Version(
            string name,
            int    major,
            int    minor,
            int    micro,
            int    build,
            string home,
            string right,
            string owner,
            int    lower,
            int    upper,
            string claim
        )
        {
            if (name == null || name.Length == 0)
                throw new System.ArgumentException("name");
            if (major < 0)
                throw new System.ArgumentException("major");
            if (minor < 0)
                throw new System.ArgumentException("minor");
            if (micro < 0)
                throw new System.ArgumentException("micro");
            if (build < 0)
                throw new System.ArgumentException("build");
            if (home != null && home.Length == 0)
                throw new System.ArgumentException("home");
            if (right == null || right.Length == 0)
                throw new System.ArgumentException("right");
            if (owner == null || owner.Length == 0)
                throw new System.ArgumentException("owner");
            if (lower > upper || lower < 1970)
                throw new System.ArgumentException("lower");
            if (upper > 9999)
                throw new System.ArgumentException("upper");
            if (claim == null || claim.Length == 0)
                throw new System.ArgumentException("claim");

            _name  = name;
            _major = major;
            _minor = minor;
            _micro = micro;
            _build = build;
            _home  = home;
            _right = right;
            _owner = owner;
            _lower = lower;
            _upper = upper;
            _claim = claim;
        }
    }
}
