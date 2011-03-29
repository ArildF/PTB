Feature: Tooltips
	In order to get more information about tasks
	As a user
	I want tooltips to show me information

Background:
    Given that the thread culture is 'nb-no'
	And that I have created a new database

Scenario: Tooltip 1
	Given a task with the following attributes:
    |Property   |Value          |
    |CreatedDate|26.01.2005 15:55   |
    |StartedDate|27.01.2006 8:32    |
    |StateChangedDate|27.01.2006 8:32    |
    |ModifiedDate|27.01.2006 19:23   |
    Then the tooltip should show
    """
    Created: 26.01.2005 15:55 
    Last modified: 27.01.2006 19:23
    Started: 27.01.2006 08:32
	State last changed: 27.01.2006 08:32
    """

Scenario: Tooltip 2
	Given a task with the following attributes:
    |Property   |Value          |
    |CreatedDate|26.01.2005 15:55   |
    |StartedDate|27.01.2006 8:32    |
    |CompletedDate|27.02.2006 8:32    |
    |StateChangedDate|27.02.2006 8:32    |
    |ModifiedDate|27.01.2006 19:23   |
    Then the tooltip should show
    """
    Created: 26.01.2005 15:55 
    Last modified: 27.01.2006 19:23
    Started: 27.01.2006 08:32
    Completed: 27.02.2006 08:32
    State last changed: 27.02.2006 08:32
    """

Scenario: Tooltip 3
	Given a task with the following attributes:
    |Property   |Value          |
    |CreatedDate|26.01.2005 15:55   |
    |StartedDate|27.01.2006 8:32    |
    |AbandonedDate|27.02.2006 8:32    |
    |StateChangedDate|27.02.2006 8:32    |
    |ModifiedDate|27.01.2006 19:23   |
    Then the tooltip should show
    """
    Created: 26.01.2005 15:55 
    Last modified: 27.01.2006 19:23
    Started: 27.01.2006 08:32
    Abandoned: 27.02.2006 08:32
    State last changed: 27.02.2006 08:32
	"""