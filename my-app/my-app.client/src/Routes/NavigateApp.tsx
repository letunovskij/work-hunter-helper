import React from 'react';
import { BrowserRouter as Router, Routes, Route, Navigate, Link } from 'react-router-dom';

//import Container from 'react-bootstrap/Container';
//import Nav from 'react-bootstrap/Nav';
//import Navbar from 'react-bootstrap/Navbar';
//import Button from 'react-bootstrap/Button';

const PrivatePage: React.FC = () => {
    const isAuthenticated = false; // Пример проверки авторизации

    if (!isAuthenticated) {
        return <Navigate to="/login" />; // Перенаправление на страницу входа
    }

    return <h1>Приватная страница</h1>;
};

const LoginPage: React.FC = () => {
    return <h1>Страница входа</h1>;
};

const RegisterPage: React.FC = () => {
    return <h1>Страница регистрации</h1>;
};

const HomePage: React.FC = () => {
    return <h1>Главная страница</h1>;
};

// Компонент для страницы 404
const NotFoundPage: React.FC = () => {
    return <h1>Страница не найдена</h1>;
};

// Основной компонент приложения
export const NavigateApp: React.FC = () => {
    return (
        <Router>
            <nav className="navbar">
                <ul className="navbar-links" role="nav">
                    <li>
                        <Link to="/">Главная</Link>
                    </li>
                    <li>
                        <Link to="/register">Зарегистрироваться</Link>
                    </li>
                    <li>
                        <Link to="/login">Войти</Link>
                    </li>
                    <li>
                        <Link to="/private">Список откликов</Link>
                    </li>
                </ul>
            </nav>
            {/*<nav class="navbar navbar-default navbar-inverse" role="navigation">*/}
            {/*    <div class="container-fluid">*/}
            {/*        <div class="navbar-header">*/}
            {/*            <button type="button" class="navbar-toggle collapsed" data-toggle="collapse" data-target="#bs-example-navbar-collapse-1">*/}
            {/*                <span class="sr-only">Toggle navigation</span>*/}
            {/*                <span class="icon-bar"></span>*/}
            {/*                <span class="icon-bar"></span>*/}
            {/*                <span class="icon-bar"></span>*/}
            {/*            </button>*/}
            {/*            <a class="navbar-brand" href="#">Login dropdown</a>*/}
            {/*        </div>*/}

            {/*        <div class="collapse navbar-collapse" id="bs-example-navbar-collapse-1">*/}
            {/*            <ul class="nav navbar-nav">*/}
            {/*                <li class="active"><a href="#">Link</a></li>*/}
            {/*                <li><a href="#">Link</a></li>*/}
            {/*                <li class="dropdown">*/}
            {/*                    <a href="#" class="dropdown-toggle" data-toggle="dropdown">Dropdown <span class="caret"></span></a>*/}
            {/*                    <ul class="dropdown-menu" role="menu">*/}
            {/*                        <li><a href="#">Action</a></li>*/}
            {/*                        <li><a href="#">Another action</a></li>*/}
            {/*                        <li><a href="#">Something else here</a></li>*/}
            {/*                        <li class="divider"></li>*/}
            {/*                        <li><a href="#">Separated link</a></li>*/}
            {/*                        <li class="divider"></li>*/}
            {/*                        <li><a href="#">One more separated link</a></li>*/}
            {/*                    </ul>*/}
            {/*                </li>*/}
            {/*            </ul>*/}
            {/*            <form class="navbar-form navbar-left" role="search">*/}
            {/*                <div class="form-group">*/}
            {/*                    <input type="text" class="form-control" placeholder="Search"/>*/}
            {/*                </div>*/}
            {/*                <button type="submit" class="btn btn-default">Submit</button>*/}
            {/*            </form>*/}
            {/*            <ul class="nav navbar-nav navbar-right">*/}
            {/*                <li><p class="navbar-text">Already have an account?</p></li>*/}
            {/*                <li class="dropdown">*/}
            {/*                    <a href="#" class="dropdown-toggle" data-toggle="dropdown"><b>Login</b> <span class="caret"></span></a>*/}
            {/*                    <ul id="login-dp" class="dropdown-menu">*/}
            {/*                        <li>*/}
            {/*                            <div class="row">*/}
            {/*                                <div class="col-md-12">*/}
            {/*                                    Login via*/}
            {/*                                    <div class="social-buttons">*/}
            {/*                                        <a href="#" class="btn btn-fb"><i class="fa fa-facebook"></i> Facebook</a>*/}
            {/*                                        <a href="#" class="btn btn-tw"><i class="fa fa-twitter"></i> Twitter</a>*/}
            {/*                                    </div>*/}
            {/*                                    or*/}
            {/*                                    <form class="form" role="form" method="post" action="login" accept-charset="UTF-8" id="login-nav">*/}
            {/*                                        <div class="form-group">*/}
            {/*                                            <label class="sr-only" for="exampleInputEmail2">Email address</label>*/}
            {/*                                            <input type="email" class="form-control" id="exampleInputEmail2" placeholder="Email address" required/>*/}
            {/*                                        </div>*/}
            {/*                                        <div class="form-group">*/}
            {/*                                            <label class="sr-only" for="exampleInputPassword2">Password</label>*/}
            {/*                                            <input type="password" class="form-control" id="exampleInputPassword2" placeholder="Password" required/>*/}
            {/*                                                <div class="help-block text-right"><a href="">Forget the password ?</a></div>*/}
            {/*                                        </div>*/}
            {/*                                        <div class="form-group">*/}
            {/*                                            <button type="submit" class="btn btn-primary btn-block">Sign in</button>*/}
            {/*                                        </div>*/}
            {/*                                    </form>*/}
            {/*                                </div>*/}
            {/*                                <div class="bottom text-center">*/}
            {/*                                    New here ? <a href="#"><b>Join Us</b></a>*/}
            {/*                                </div>*/}
            {/*                            </div>*/}
            {/*                        </li>*/}
            {/*                    </ul>*/}
            {/*                </li>*/}
            {/*            </ul>*/}
            {/*        </div>*/}{/*.navbar-collapse*/}
            {/*    </div>*/}{/*.container-fluid*/}
            {/*</nav>*/}
            <Routes>
                <Route path="/" element={<HomePage />} />
                <Route path="/private" element={<PrivatePage />} />
                <Route path="/login" element={<LoginPage />} />
                <Route path="/register" element={<RegisterPage />} />
                <Route path="*" element={<NotFoundPage />} /> {/* Страница 404 */}
            </Routes>
        </Router>
    );
};