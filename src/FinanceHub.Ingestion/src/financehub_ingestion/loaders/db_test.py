import psycopg2


def test_connection() -> None:
    """
    Tests the PostgreSQL connection for the FinanceHub project.
    """
    try:
        connection = psycopg2.connect(
            host="localhost",
            database="financehub",
            user="financehub",
            password="financehub",
            port=5432
        )

        print("Database connection successful!")
        connection.close()
    except Exception as ex:
        raise RuntimeError("Database connection failed.") from ex


if __name__ == "__main__":
    test_connection()