namespace VB.DBManager.NHConfig.UserPassed
{
    /// <summary>
    /// Inherit this class and pass yours to ConnectionInfo class to fill the DB after create.
    /// The property CreateDB need to be TRUE.
    /// </summary>
    public interface IDataInitializer
    {
        /// <summary>
        /// Fill the Database.
        /// </summary>
        void Initialize();
    }
}
