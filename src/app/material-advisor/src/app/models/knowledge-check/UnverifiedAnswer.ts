import { SubmittedAnswer } from "./SubmittedAnswer";
import { Topic } from "@models/topic/Topic";
import { KnowledgeCheckTopicListItem } from "./KnowledgeCheckTopicListItem";
import { User } from "@shared/models/User";

export interface UnverifiedAnswer extends SubmittedAnswer {
  topic: Topic;
  knowledgeCheck: KnowledgeCheckTopicListItem;
  user: User;
}
