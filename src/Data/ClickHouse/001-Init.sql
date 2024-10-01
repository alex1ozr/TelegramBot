CREATE TABLE IF NOT EXISTS SpaceWeather.tbot_user_commands
(
    user_id String,
    command String,
    timestamp DateTime
)
ENGINE = MergeTree()
    PARTITION BY toYYYYMM(timestamp)
    ORDER BY (user_id, timestamp);
