namespace HiperNFe.Models;

/// <summary>
/// Tipos de manifestação do destinatário.
/// </summary>
public enum NFeManifestationType
{
    ConfirmationOfOperation = 210200,
    IgnoranceOfOperation = 210210,
    OperationNotPerformed = 210220,
    OperationPerformed = 210240
}
