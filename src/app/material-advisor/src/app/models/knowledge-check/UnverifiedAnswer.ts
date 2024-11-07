import { SubmittedAnswer } from "./SubmittedAnswer";
import { Topic } from "@models/topic/Topic";
import { KnowledgeCheckTopicListItem } from "./KnowledgeCheckTopicListItem";
import { User } from "@shared/models/User";
import { VerifiedAnswer } from "./VerifiedAnswer";

export interface UnverifiedAnswer {
  verifiedAnswers: VerifiedAnswer[];
  submittedAnswer: SubmittedAnswer;
  topic: Topic;
  knowledgeCheck: KnowledgeCheckTopicListItem;
  user: User;
}