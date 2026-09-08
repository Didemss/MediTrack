using MediTrack.Domain.Entities;

namespace MediTrack.Application.Interfaces;

public interface ITokenService
{
    string GenerateToken(User user);
}