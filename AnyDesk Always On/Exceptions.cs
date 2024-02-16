namespace AnyDesk_Always_On;

public class ProcessCapturingException(Exception exception) : Exception(@"Houve um erro ao capturar os processos referentes ao AnyDesk Always On", exception)
{ }
