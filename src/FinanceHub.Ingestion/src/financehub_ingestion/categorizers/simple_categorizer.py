def categorize(description: str) -> str:
    """
    Simple rule-based categorization based on transaction description keywords.
    """

    desc = description.lower()

    if any(word in desc for word in ["supermarket", "aldi", "lidl"]):
        return "Groceries"

    if any(word in desc for word in ["netflix", "spotify", "cinema"]):
        return "Entertainment"

    if any(word in desc for word in ["salary", "income"]):
        return "Income"

    if any(word in desc for word in ["coffee", "cafe", "restaurant"]):
        return "Food & Drinks"

    if any(word in desc for word in ["train", "ticket", "uber"]):
        return "Transport"

    if any(word in desc for word in ["electricity", "water bill", "gas bill"]):
        return "Utilities"

    if any(word in desc for word in ["amazon"]):
        return "Shopping"

    if any(word in desc for word in ["gym", "fitness"]):
        return "Health & Fitness"

    return "Other"