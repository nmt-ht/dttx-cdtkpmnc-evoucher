using System;
using System.Collections.Generic;

namespace eVoucher.Models;
public class CampaignGame
{
    public Guid ID { get; set; }
    public Game Game { get; set; }
    public int Index { get; set; }
    public string Name => Game != null ? Game.Name : string.Empty;
    public string Description => Game != null ? Game.Description : string.Empty;
}
