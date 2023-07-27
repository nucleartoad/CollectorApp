import { useRef, useState, useEffect } from "react";
import { faCheck, faTimes, faInfoCircle } from "@fortawesome/free-solid-svg-icons";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";

import axios from "../api/axios";
const REGISTER_URL = "/Authentication/Register";

const USER_REGEX = /^[a-zA-Z][a-zA-Z0-9-_]{3,23}$/;
const PWD_REGEX = /^(?=.*[!@#$%])(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9]).{8,24}$/;

const Register = () => {
    const userRef = useRef();
    const errRef = useRef();

    const [username, setUser] = useState("");
    const [validName, setValidName] = useState(false);
    const [userFocus, setUserFocus] = useState(false);

    const [password, setPassword] = useState("");
    const [validPassword, setValidPassword] = useState(false);
    const [passwordFocus, setPasswordFocus] = useState(false);

    const [matchPassword, setMatchPassword] = useState("");
    const [validMatch, setValidMatch] = useState(false);
    const [matchFocus, setMatchFocus] = useState(false);

    const [errorMessage, setErrorMessage] = useState("");
    const [success, setSuccess] = useState(false);

    useEffect(() => {
        userRef.current.focus();
    }, [])

    useEffect(() => {
        const result = USER_REGEX.test(username);
        console.log(result);
        console.log(username);
        setValidName(result);
    }, [username])

    useEffect(() => {
        const result = PWD_REGEX.test(password);
        console.log(result);
        console.log(password);
        setValidPassword(result);
        const match = password === matchPassword;
        setValidMatch(match);
    }, [password, matchPassword])

    useEffect(() => {
        setErrorMessage("");
    }, [username, password, matchPassword])

    const handleSubmit = async (e) => {
        e.preventDefault(0);
        // if button enabled with JS hack
        const v1 = USER_REGEX.test(username);
        const v2 = PWD_REGEX.test(password);
        if (!v1 || !v2) {
            setErrorMessage("Invalid Entry");
        }
        try {
            const response = await axios.post(REGISTER_URL, JSON.stringify({username, password}), {
                headers: { "Content-Type": "application/json" },
                withCredentials: true,
            });
            console.log(response.data);
            console.log(response.data.token);
            console.log(JSON.stringify(response));
            setUser('');
            setPassword('');
            setSuccess(true);
            console.log(success);
        } catch (error) {
            if (!error?.response) {
                setErrorMessage("No Sever Response");
            } else if (error.response?.status === 409) {
                setErrorMessage("Username Taken");
            } else {
                setErrorMessage("Registration Failed");
            }
            errRef.current.focus(); // for screen readers
        }
    }

    return (
        <div>
            <p ref={errRef} className={errorMessage ? "errormessage" : "hide"} aria-live="assertive">
                {errorMessage}
            </p>
            <h1>Register</h1>
            <form onSubmit={handleSubmit}>
                <label htmlFor="username">
                    Username:
                    <span className={validName ? "valid" : "hide"}>
                        <FontAwesomeIcon icon={faCheck} />
                    </span>
                    <span className={validName || !username ? "hide" : "invalid"}>
                        <FontAwesomeIcon icon={faTimes} />
                    </span>
                </label>
                <input
                    type="text"
                    id="username"
                    ref={userRef}
                    autoComplete="off"
                    onChange={(e) => setUser(e.target.value)}
                    required
                    aria-invalid={validName ? "false" : "true"}
                    aria-describedby="uidnote"
                    onFocus={() => setUserFocus(true)}
                    onBlur={() => setUserFocus(false)}
                />
                <p id="uidnote" className={userFocus && username && !validName ? "instructions" : "hide"}>
                    <FontAwesomeIcon icon={faInfoCircle} />
                    4 to 24 characters.
                    <br />
                    Must begin with a letter.
                    <br />
                    Letters, Numbers, Underscores, Hyphens allowed.
                </p>

                <label htmlFor="password">
                    Password:
                    <span className={validPassword ? "valid" : "hide"}>
                        <FontAwesomeIcon icon={faCheck} />
                    </span>
                    <span className={validPassword || !password ? "hide" : "invalid"}>
                        <FontAwesomeIcon icon={faTimes} />
                    </span>
                </label>
                <input
                    type="password"
                    id="password"
                    onChange={(e) => setPassword(e.target.value)}
                    required
                    aria-invalid={validPassword ? "false" : "true"}
                    aria-describedby="pwdnote"
                    onFocus={() => setPasswordFocus(true)}
                    onBlur={() => setPasswordFocus(false)}
                />
                <p id="pwdnote" className={passwordFocus && !validPassword ? "instructions" : "hide"}>
                    <FontAwesomeIcon icon={faInfoCircle} />
                    8 to 24 characters.
                    <br />
                    Must include uppercase and lowercase letters, a number and a special character.
                    <br />
                    Allowed special characters: <span aria-label="exclamation mark">!</span> <span aria-label="at symbol">@</span> <span aria-label="hashtag">#</span>{" "}
                    <span aria-label="dollar sign">$</span> <span aria-label="percent">%</span>
                </p>

                <label htmlFor="confirmPassword">
                    Confirm Password:
                    <span className={validMatch && matchPassword ? "valid" : "hide"}>
                        <FontAwesomeIcon icon={faCheck} />
                    </span>
                    <span className={validMatch || !matchPassword ? "hide" : "invalid"}>
                        <FontAwesomeIcon icon={faTimes} />
                    </span>
                </label>
                <input
                    type="password"
                    id="confirmPassword"
                    onChange={(e) => setMatchPassword(e.target.value)}
                    required
                    aria-invalid={validMatch ? "false" : "true"}
                    aria-describedby="confirmnote"
                    onFocus={() => setMatchFocus(true)}
                    onBlur={() => setMatchFocus(false)}
                />
                <p id="confirmnote" className={matchFocus && !validMatch ? "instructions" : "hide"}>
                    <FontAwesomeIcon icon={faInfoCircle} />
                    Must match the first password input field.
                </p>

                <button disabled={!validName || !validPassword || !validMatch ? true : false}>Sign Up</button>
            </form>

            <p>
                Already registered?
                <br />
                <span className="line">
                    {/*put router link here*/}
                    <a href="#">Sign In</a>
                </span>
            </p>
        </div>
    );
};

export default Register;
