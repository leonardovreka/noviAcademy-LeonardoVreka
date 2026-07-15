using Domain.Enums;

namespace Api.DTOs;

public record CreateWalletRequest(int PlayerId, Currency Currency, decimal InitialBalance);