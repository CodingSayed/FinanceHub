def categorize(description: str) -> str:
    """
    Simple rule-based categorization based on keywords.
    """

    desc = description.lower()

    if any(word in desc for word in ["supermarket", "aldi", "lidl"]):
        return "Groceries"

    if any(word in desc for word in ["netflix", "spotify"]):
        return "Entertainment"

    if any(word in desc for word in ["salary", "income"]):
        return "Income"

    if any(word in desc for word in ["coffee", "cafe"]):
        return "Food & Drinks"

    return "Other"