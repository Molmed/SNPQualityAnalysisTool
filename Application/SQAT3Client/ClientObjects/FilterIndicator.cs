namespace Molmed.SQAT.ClientObjects
{

    public class FilterIndicator
    {
        private bool MyPlate;
        private bool MyGenotypeGroup;
        private bool MySampleGroup;
        private bool MyIndividualGroup;
        private bool MyAssayGroup;
        private bool MyMarkerGroup;
        private bool MySample;
        private bool MyAssay;
        private bool MyIndividual;
        private bool MyMarker;

        public FilterIndicator()
        {
            MyPlate = false;
            MyGenotypeGroup = false;
            MySampleGroup = false;
            MyIndividualGroup = false;
            MyAssayGroup = false;
            MyMarkerGroup = false;
            MySample = false;
            MyAssay = false;
            MyIndividual = false;
            MyMarker = false;
        }

        public bool Plate { get { return MyPlate; } set { MyPlate = value; } }
        public bool GenotypeGroup { get { return MyGenotypeGroup; } set { MyGenotypeGroup = value; } }
        public bool SampleGroup { get { return MySampleGroup; } set { MySampleGroup = value; } }
        public bool IndividualGroup { get { return MyIndividualGroup; } set { MyIndividualGroup = value; } }
        public bool AssayGroup { get { return MyAssayGroup; } set { MyAssayGroup = value; } }
        public bool MarkerGroup { get { return MyMarkerGroup; } set { MyMarkerGroup = value; } }
        public bool Sample { get { return MySample; } set { MySample = value; } }
        public bool Assay { get { return MyAssay; } set { MyAssay = value; } }
        public bool Individual { get { return MyIndividual; } set { MyIndividual = value; } }
        public bool Marker { get { return MyMarker; } set { MyMarker = value; } } 
    
    }


}