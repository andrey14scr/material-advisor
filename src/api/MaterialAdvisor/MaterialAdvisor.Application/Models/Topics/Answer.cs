﻿namespace MaterialAdvisor.Application.Models.Topics;

public class Answer
{
    public byte Number { get; set; }

    public double Points { get; set; }

    public bool IsRight { get; set; }

    public IEnumerable<LanguageText> Content { get; set; } = [];
}
