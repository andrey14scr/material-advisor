﻿namespace MaterialAdvisor.Application.Models.KnowledgeChecks;

public class SubmittedAnswer
{
    public Guid AnswerGroupId { get; set; }

    public Guid AttemptId { get; set; }

    public string? Value { get; set; }
}