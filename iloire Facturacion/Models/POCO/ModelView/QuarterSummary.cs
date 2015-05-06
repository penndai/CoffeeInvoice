using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

public class QuarterSummary
{
    public int Year { get; set; }

    public Summary Month1 { get; set; }
    public Summary Month2 { get; set; }
    public Summary Month3 { get; set; }
}

public class TransQuarterSummary
{
	public int Year { get; set; }

	public TSummary Month1 { get; set; }
	public TSummary Month2 { get; set; }
	public TSummary Month3 { get; set; }
}