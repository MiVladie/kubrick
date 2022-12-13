import React from 'react';

import { useLocation } from 'react-router-dom';

import classes from './Layout.module.scss';

import './reset.css';

interface Props {
	children: React.ReactNode;
}

const Layout = ({ children }: Props) => {
	const location = useLocation();

	const getClass = (): string => {
		switch (location.pathname) {
			case '/play':
				return classes.Play;

			default:
				return '';
		}
	};

	return (
		<div className={classes.Layout}>
			<div className={[classes.Background, getClass()].join(' ')} />

			<div className={classes.Content}>{children}</div>
		</div>
	);
};

export default Layout;
