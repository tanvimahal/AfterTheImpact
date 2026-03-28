const express = require("express");
const { Pool } = require("pg");
const cors = require("cors");
const bcrypt = require("bcrypt");

const app = express();
app.use(cors());
app.use(express.json());

const pool = new Pool({
    connectionString: process.env.DB_URL,
    ssl: {
        rejectUnauthorized: false
    }
});


app.post("/save", async (req, res) => {
    const { username, password, cycle, food, saplings, wood, totalScore, totalTreesCut, totalTreesPlanted, totalAnimalsKilled, totalBuildingsBuilt} = req.body;

    if (!username || !password || cycle === undefined) {
        return res.status(400).json({ error: "Missing username, password, or cycle" });
    }

    try {
        
        let userResult = await pool.query("SELECT id, password FROM users WHERE username=$1", [username]);
        let userId;

        if (userResult.rows.length === 0) {
            return res.status(404).json({ error: "User not found" });
        } else {
            
            const isMatch = await bcrypt.compare(password, userResult.rows[0].password);

            if (!isMatch) {
                return res.status(403).json({ error: "Incorrect password" });
            }
            userId = userResult.rows[0].id;
        }

        
        await pool.query(
            `INSERT INTO saves (user_id, disaster_cycle, food_count, sapling_count, wood_count, total_score, total_trees_cut, total_trees_planted, total_animals_killed, total_buildings_built)
             VALUES ($1, $2, $3, $4, $5, $6, $7, $8, $9, $10)
             ON CONFLICT (user_id)
             DO UPDATE SET 
             disaster_cycle = EXCLUDED.disaster_cycle,
             food_count = EXCLUDED.food_count,
             sapling_count = EXCLUDED.sapling_count,
             wood_count = EXCLUDED.wood_count,
             total_score = EXCLUDED.total_score,
             total_trees_cut = EXCLUDED.total_trees_cut,
             total_trees_planted = EXCLUDED.total_trees_planted,
             total_animals_killed = EXCLUDED.total_animals_killed,
             total_buildings_built = EXCLUDED.total_buildings_built`,
            [userId, cycle, food, saplings, wood, totalScore, totalTreesCut, totalTreesPlanted, totalAnimalsKilled, totalBuildingsBuilt]
        );

        res.json({ message: "Save successful" });
    } catch (err) {
        console.error(err);
        res.status(500).json({ error: "Server error" });
    }
});


app.post("/load", async (req, res) => {
    const { username, password } = req.body;

    if (!username || !password) {
        return res.status(400).json({ error: "Missing username or password" });
    }

    try {
        const userResult = await pool.query(
            "SELECT id, password FROM users WHERE username=$1",
            [username]
        );

        if (userResult.rows.length === 0) {
            return res.json({
                disaster_cycle: 0,
                food_count: 0,
                sapling_count: 0,
                wood_count: 0
            });
        }

        const isMatch = await bcrypt.compare(password, userResult.rows[0].password);

        if (!isMatch) {
            return res.status(403).json({ error: "Incorrect password" });
        }

        const saveResult = await pool.query(
            `SELECT disaster_cycle, food_count, sapling_count, wood_count, total_score, total_trees_cut, total_trees_planted, total_animals_killed, total_buildings_built
             FROM saves 
             WHERE user_id=$1`,
            [userResult.rows[0].id]
        );

        if (saveResult.rows.length === 0) {
            return res.json({
                disaster_cycle: 0,
                food_count: 0,
                sapling_count: 0,
                wood_count: 0,
                total_score: 0,
                total_trees_cut: 0,
                total_trees_planted: 0,
                total_animals_killed: 0,
                total_buildings_built: 0
            });
        }

        res.json(saveResult.rows[0]);

    } catch (err) {
        console.error(err);
        res.status(500).json({ error: "Server error" });
    }
});

app.post("/login", async (req, res) => {
    const { username, password } = req.body;

    if (!username || !password) {
        return res.status(400).json({ error: "Missing username or password" });
    }

    try {
        let userResult = await pool.query(
            "SELECT id, password FROM users WHERE username=$1",
            [username]
        );

        if (userResult.rows.length === 0) {
            return res.status(404).json({ error: "User not found" });
        }

        
        const isMatch = await bcrypt.compare(password, userResult.rows[0].password);

        if (!isMatch) {
            return res.status(403).json({ error: "Incorrect password" });
        }

        res.json({ success: true, userId: userResult.rows[0].id });

    } catch (err) {
        console.error("LOGIN ERROR:", err); 
        res.status(500).json({ error: "Server error" });
    }
});

app.post("/signup", async (req, res) => {
    const { username, email, password } = req.body;

    if (!username || !email || !password) {
        return res.status(400).json({ error: "Missing fields" });
    }

    try {
        
        const existingUser = await pool.query(
            "SELECT id FROM users WHERE username=$1",
            [username]
        );

        if (existingUser.rows.length > 0) {
            return res.status(409).json({ error: "Username already exists" });
        }

        
        const hashedPassword = await bcrypt.hash(password, 10);

        const result = await pool.query(
            "INSERT INTO users (username, email, password) VALUES ($1, $2, $3) RETURNING id",
            [username, email, hashedPassword]
        );

        res.json({ success: true, userId: result.rows[0].id });

    } catch (err) {
        console.error("SIGNUP ERROR:", err);
        res.status(500).json({ error: "Server error" });
    }
});


const PORT = process.env.PORT || 3000;
app.listen(PORT, () => console.log(`Server running on port ${PORT}`));