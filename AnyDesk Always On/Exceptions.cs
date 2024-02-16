namespace AnyDesk_Always_On;

public class ProcessCapturingException(Exception exception) : Exception(@"Houve um erro ao capturar os processos referentes ao AnyDesk Always On", exception)
{ }

public class HandledException(Exception exception) : Exception("Erro Capturado com Sucesso! Contate seu TI\nErro:", exception);
