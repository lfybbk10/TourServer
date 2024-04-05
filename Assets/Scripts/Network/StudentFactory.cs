using Mirror;

public class StudentFactory
{
    public Student Create(NetworkConnectionToClient conn)
    {
        Student student = new Student(NameGenerator.GetRandomName(), conn);
        return student;
    }
}
