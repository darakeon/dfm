namespace VB.DBManager.NHConfig.Helpers
{
    /// <summary>
    /// List of accepted DBMSs
    /// </summary>
    public enum DBMS
    {
        ///<summary></summary>
        MySQL,
        ///<summary></summary>
        MsSql2000,
        ///<summary></summary>
        MsSql2005,
        ///<summary></summary>
        MsSql2008,
        ///<summary></summary>
        MsSql7,
        /// <summary>Doesn't need SERVER property</summary>
        Postgre,
        /// <summary>Doesn't need DATABASE property</summary>
        Oracle9,
        /// <summary>Doesn't need DATABASE property</summary>
        Oracle10,
        /// <summary>Doesn't need DATABASE property</summary>
        SQLite,
    }

    ///<summary>
    /// Action for DB when create SessionFactory
    ///</summary>
    public enum DBAction
    {
        ///<summary> 
        /// Don't even validate the schema.
        /// The errors will appear just when the entity is accessed.
        ///</summary>
        None,
        ///<summary>
        /// Drop and Create the DB.
        /// The saved data will be LOST.
        ///</summary>
        Recreate,
        ///<summary>
        /// Just ajust the DB to match the entities.
        /// Can recreate foreign keys.
        ///</summary>
        Update,
        ///<summary>
        /// Verify errors on entities and mapping.
        /// Avoid error to just appear when entities are accessed.
        ///</summary>
        Validate,
    }
}
