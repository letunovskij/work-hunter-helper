import { Component } from 'react';
import { DemoContext } from './DemoContextProvider';

export class LevelThree extends Component {
    render() {
        return (
            <DemoContext.Consumer>
                {context => {
                    if (!context) {
                        return <div>Ошибка: Контекст не найден</div>;
                    }
                    const { message, updateMessage } = context;
                    const currentDate = new Date().toLocaleString();
                    return (
                        <div>
                            <h4>Уровень 3</h4>
                            <p>{message}</p>
                            <button onClick={() => updateMessage(`Сообщение из LevelThree (${currentDate})`)}>Сообщение с датой</button>
                        </div>
                    );
                }}
            </DemoContext.Consumer>
        );
    }
}