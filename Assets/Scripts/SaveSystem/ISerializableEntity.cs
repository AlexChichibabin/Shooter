public interface ISerializableEntity
{
    long EntityId { get; }
    bool IsSerializable();
    string SerializeState();
    void DeserializeState(string state);
}
