using MP.Multitasking.Common.OutputManagers;
using MP.Multitasking.Tasks.Math;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;

namespace MP.Multitasking.Tasks
{
    public class CollectionTasksImplementation
    {
        private const int CollectionCapacity = 10;

        private readonly object _lockedObject;
        private readonly ObservableCollection<int> _collection;

        private readonly IOutputManager _outputManager;

        public CollectionTasksImplementation(IOutputManager outputManager)
        {
            _outputManager = outputManager ?? throw new ArgumentNullException(nameof(outputManager));

            _collection = new ObservableCollection<int>();
            _lockedObject = new object();
        }

        public void Task6()
        {
            var valuesToAdd = Enumerable.Range(0, CollectionCapacity).Select(item => MathHelper.RandomNumber).ToList();

            var listenerThread = Task.Run(() => _collection.CollectionChanged += SafelyPrintCollectionValues);
            var writterThread = Task.Run(() => SafelyAddToCollection(valuesToAdd));

            Task.WaitAll(listenerThread, writterThread);
        }

        #region Private methods

        private void SafelyAddToCollection(List<int> values)
        {
            values.ForEach(item => SafelyAddToCollection(item));
        }

        private void SafelyAddToCollection(int value)
        {
            lock (_lockedObject)
            {
                _collection.Add(value);
            }
        }

        private void SafelyPrintCollectionValues(object sender, NotifyCollectionChangedEventArgs e)
        {
            lock (_lockedObject)
            {
                _outputManager.DisplayMessage($"Items in collection: {_collection.Count}");

                foreach (var item in _collection)
                {
                    _outputManager.DisplayMessage(item.ToString());
                }
            }
        }

        #endregion
    }
}
