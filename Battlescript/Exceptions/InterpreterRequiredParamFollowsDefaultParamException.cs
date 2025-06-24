namespace Battlescript;

public class InterpreterRequiredParamFollowsDefaultParamException() : Exception(
    "SyntaxError: non-default argument follows default argument");