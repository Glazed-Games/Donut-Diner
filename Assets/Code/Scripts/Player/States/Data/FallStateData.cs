using DonutDiner.PlayerModule.States.DTOs;

namespace DonutDiner.PlayerModule.States.Data
{
    public class FallStateData : PlayerStateData
    {
        #region Properties

        public int JumpsRemaining { get; set; } = 1;
        public float TimeFalling { get; set; }
        public float CoyoteTimeCounter { get; set; }

        #endregion

        #region Constructor

        public FallStateData(PlayerActionDTO dto) : base(dto)
        {
        }

        #endregion
    }
}