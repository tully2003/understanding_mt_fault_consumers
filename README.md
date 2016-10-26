# Understanding MassTransit Fault Consumer
The solution *MT_FAULT_CONSUMERS* contains 3 console applications each acting as there own "Bounded Contexts"

## BoundedContextA
ContextA publishes the EventThatOccurredInA message.

## BoundedContextB
ContextB contains an event consumer which is running on receive endpoint listening to "queue_b". 
The **EventConsumer** handles both `EventThatOccurredInA` and `Fault<EventThatOccurredInA>`. 
When processing `EventThatOccurredInA` the consumer throws an exception, I expect the consumer 
`Fault<EventThatOccurredInA>` to pick this up

## BoundedContextC
ContextB contains an event consumer which is running on receive endpoint listening to "queue_c". 
The **EventConsumer** handles both `EventThatOccurredInA` and `Fault<EventThatOccurredInA>`. 
I do **not** expect the consumer for `Fault<EventThatOccurredInA>` to be triggered by context.

## Expectations
I expect that when consuming events in ContextB and the handling fails for the fault to only be processed by ContextB