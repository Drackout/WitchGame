namespace Assets.DialogueSystem.Editor
{
    /// <summary>
    /// Class responsible for storing the data of a Dialogue Choice
    /// </summary>
    public struct OutportData
    {
        
        /// <summary>
        /// Property that defines the text of the choice 
        /// </summary>
        public string Name { get; }
        
        
        /// <summary>
        /// Property that defines the unique ID of the choice
        /// </summary>
        public string ID { get; }

        
        /// <summary>
        /// Construtor for he class
        /// </summary>
        /// <param name="name"></param>
        /// <param name="id"></param>
        public OutportData(string name, string id)
        {
            Name = name;
            ID = id;
        }

    }
}
