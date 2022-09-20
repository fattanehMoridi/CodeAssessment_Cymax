namespace RequestingAPIs
{

        public class InputRequest
        {
           public  string SurceAddress;
            public string DestinationAddress; 
            public double[] CartonDimensions = new double[3]; 
            public InputRequest(string surceAddress, string destinationAddress, double[] cartonDimensionss) 
            {
                SurceAddress = surceAddress;
                DestinationAddress = destinationAddress;
                CartonDimensions = cartonDimensionss;
            }
        }
}
