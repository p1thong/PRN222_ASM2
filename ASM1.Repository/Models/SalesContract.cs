using System;
using System.Collections.Generic;

namespace ASM1.Repository.Models;

public partial class SalesContract
{
    public int SaleContractId { get; set; }

    public int OrderId { get; set; }

    public DateOnly? SignedDate { get; set; }

    public virtual Order Order { get; set; } = null!;
}
