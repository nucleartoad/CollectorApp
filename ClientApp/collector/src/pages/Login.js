import { useRef, useState, useEffect, useContext } from "react";
import { Link, useNavigate, useLocation } from 'react-router-dom';
import { userContext } from "../context/userContext";

import axios from '../api/axios';
const LOGIN_URL = '/Authentication/Login';

const Login = () => {
    const { user, setUser } = useContext(userContext);

    const navigate = useNavigate();
    const location = useLocation();
    const from = location.state?.from?.pathname || "/";

    const userRef = useRef();
    const errRef = useRef();

    const [username, setUsername] = useState("");
    const [password, setPassword] = useState("");
    const [errorMessage, setErrorMessage] = useState("");

    useEffect(() => {
        userRef.current.focus();
    }, []);

    useEffect(() => {
        setErrorMessage("");
    }, [username, password]);

    const handleSubmit = async (e) => {
        e.preventDefault();

        try {
            const response = await axios.post(LOGIN_URL, 
                JSON.stringify({username, password}),
                {
                    headers: { 'Content-Type': 'application/json'},
                    withCredentials: true
                }
            );
            console.log(JSON.stringify(response?.data.token));
            const token = response?.data?.token;
            const refreshToken = response?.data?.refreshToken;

            console.log(token);
            console.log(refreshToken);

            const user = ({ 
                username: username, 
                token: token,
                refreshToken: refreshToken
            });
            setUser(user);

            console.log(JSON.stringify(user));

            setUsername('');
            setPassword('');
            navigate(from, { replace: true });
        } catch (error) {
            if (!error?.response) {
                setErrorMessage('No Server Response');
            } else if (error.response?.status === 400) {
                setErrorMessage('Missing Username or Password');
            } else if (error.response?.status === 401) {
                setErrorMessage('Unauthorized');
            } else {
                setErrorMessage('Login Failed');
            }
            errRef.current.focus();
        }
    }

    return (
        <div>
            <p ref={errRef} className={errorMessage ? "errormessage" : "hide"} aria-live="assertive">
                {errorMessage}
            </p>
            <h1>Sign In</h1>
            <form onSubmit={handleSubmit}>
                <label htmlFor="username">Username:</label>
                <input type="text" id="" ref={userRef} autoComplete="off" onChange={(e) => setUsername(e.target.value)} value={username} required />
                <label htmlFor="password">Password:</label>
                <input type="password" id="password" onChange={(e) => setPassword(e.target.value)} value={password} required />
                <button>Sign In</button>
            </form>
            <p>
                Need an Account?<br />
                <span className="line">
                    {/*put router link here*/}
                    <a href="#">Sign Up</a>
                </span>
            </p>
        </div>
    );
};

export default Login;