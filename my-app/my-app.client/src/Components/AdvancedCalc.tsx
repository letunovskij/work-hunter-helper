import React, { Component } from 'react';

interface State {
    value1: number;
    value2: number;
    result: number;
}

export class AdvancedCalc extends Component<{}, State> {
    constructor(props: {}) {
        super(props);
        this.state = {
            value1: 0,
            value2: 0,
            result: 0
        };
    }

    handleValue1Change = (e: React.ChangeEvent<HTMLInputElement>) => {
        this.setState({ value1: parseFloat(e.target.value) });
    };

    handleValue2Change = (e: React.ChangeEvent<HTMLInputElement>) => {
        this.setState({ value2: parseFloat(e.target.value) });
    };

    add = () => {
        this.setState({ result: this.state.value1 + this.state.value2 });
    };

    subtract = () => {
        this.setState({ result: this.state.value1 - this.state.value2 });
    };

    multiply = () => {
        this.setState({ result: this.state.value1 * this.state.value2 });
    };

    divide = () => {
        if (this.state.value2 !== 0) {
            this.setState({ result: this.state.value1 / this.state.value2 });
        } else {
            alert('Деление на ноль невозможно');
        }
    };

    render() {
        return (
            <div>
                <h1>Калькулятор</h1>
                <input
                    type="number"
                    value={this.state.value1}
                    onChange={this.handleValue1Change}
                />
                <input
                    type="number"
                    value={this.state.value2}
                    onChange={this.handleValue2Change}
                />
                <div>
                    <button onClick={this.add}>Сложить</button>
                    <button onClick={this.subtract}>Вычесть</button>
                    <button onClick={this.multiply}>Умножить</button>
                    <button onClick={this.divide}>Разделить</button>
                </div>
                <h2>Результат: {this.state.result}</h2>
            </div>
        );
    }
}