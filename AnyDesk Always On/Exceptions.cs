namespace AnyDesk_Always_On;

public class ProcessCapturingException(Exception exception) : Exception(@"Houve um erro ao capturar os processos referentes ao AnyDesk Always On", exception);

public class HandledException(Exception exception) : Exception("Erro Capturado com Sucesso! Contate seu TI\nErro:", exception);

public class HandledFatalException(Exception exception) : Exception("Erro Fatal Capturado com Sucesso!\nA Aplicação será Fechada, Para Ajuda, Contate seu TI\nErro:", exception);

