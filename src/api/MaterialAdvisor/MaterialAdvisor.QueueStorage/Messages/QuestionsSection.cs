﻿using MaterialAdvisor.Data.Enums;

namespace MaterialAdvisor.QueueStorage.Messages;

public class QuestionsSection
{
    public int Count { get; set; }

    public QuestionType QuestionType { get; set; }

    public byte? AnswersCount { get; set; }
}