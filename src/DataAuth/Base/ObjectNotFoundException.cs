public class ObjectNotFoundException : Exception
{
    public ObjectNotFoundException(string objectName, object id)
        : base($"Object {objectName} with id {id} not found.") { }
}
