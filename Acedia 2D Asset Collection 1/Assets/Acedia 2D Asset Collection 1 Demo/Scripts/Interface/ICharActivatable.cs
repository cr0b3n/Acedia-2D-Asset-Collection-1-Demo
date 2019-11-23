public interface ICharActivatable {

    void Active(bool isActive, int sortOrder);

    CharacterSet CharSet { get; }
}
