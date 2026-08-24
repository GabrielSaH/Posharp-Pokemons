using Pokete.Models;

namespace Pokete.Moves;

public class MoveInstance(
    Move baseMove,
    int? maxPP = null,
    int? currentPP = null
    )
{
    public Move BaseMove { get; init; } = baseMove;
    public int MaxPP { get; set; } = maxPP ?? baseMove.BasePp;
    public int CurrentPP { get; set; } = currentPP ?? (maxPP ?? baseMove.BasePp);
    private int NumberOfBoostPP { get; set; } = 0;

    public MoveResult Execute(PosharpInstance user, PosharpInstance target)
    {
        CurrentPP--;
        return BaseMove.Effect.Execute(user, target);
    }
    
    public bool IsAvailable() => CurrentPP > 0;
    public void RecoverPP(int value) => CurrentPP = Math.Clamp(CurrentPP +  value, 0, MaxPP);
    public void RecoverFullPP() => CurrentPP = MaxPP;
    
    public bool TryRaiseMaxPP()
    {
        if (!IsPPRaisable()) return false;
        BoostMaxPP();
        return true;
    }
    
    public bool IsPPRaisable() => NumberOfBoostPP < 3;
    private void BoostMaxPP()
    {
        NumberOfBoostPP++;
        MaxPP = MaxPP + (int)Math.Round((BaseMove.BasePp * 0.2));
    }
}