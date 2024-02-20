namespace AnyDeskAlwaysOn.Model;

public class ProcessCapturingException(Exception exception) : Exception(@"Houve um erro ao capturar os processos referentes ao AnyDesk Always On", exception);

public class HandledException(Exception exception) : Exception("Erro Capturado com Sucesso! Contate seu TI\nErro:", exception);

public class DuplicatedInstanceException() : Exception("Já Existe uma Instancia do Software Sendo Executada!");