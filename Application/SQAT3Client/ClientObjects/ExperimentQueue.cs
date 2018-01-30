using System;

namespace Molmed.SQAT.ClientObjects
{

    public class ExperimentQueue
    {
        private int[] MyExperimentIDs;
        private int MyNextIndex;

        public ExperimentQueue(int [] experimentIDs)
        {
            MyExperimentIDs = new int[experimentIDs.GetLength(0)];
            experimentIDs.CopyTo(MyExperimentIDs, 0);
            MyNextIndex = 0;
        }

        public int[] GetNextBatch(int batchSize)
        {
            int[] batch;

            if (MyNextIndex >= MyExperimentIDs.GetLength(0))
            {
                return null;
            }
            if ((MyNextIndex + batchSize) >= MyExperimentIDs.GetLength(0))
            {
                batch = new int[MyExperimentIDs.GetLength(0) - MyNextIndex];
            }
            else
            {
                batch = new int[batchSize];
            }
            for (int i = 0; i < batch.GetLength(0); i++)
            {
                batch[i] = MyExperimentIDs[MyNextIndex++];
            }

            return batch;
        }

        public int PercentFinished
        {
            get
            {
                if (MyExperimentIDs.GetLength(0) > 0)
                {
                    return Convert.ToInt32(Math.Floor(100 * Convert.ToDouble(MyNextIndex) / Convert.ToDouble(MyExperimentIDs.GetLength(0))));
                }
                else
                {
                    return 0;
                }
            }

        }

        public int TotalSize
        {
            get
            {
                return MyExperimentIDs.GetLength(0);
            }
        }

    }


}