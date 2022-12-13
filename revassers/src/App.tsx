import React from 'react';

import { Routes, Route } from 'react-router-dom';
import { Helmet } from 'react-helmet';

import Layout from 'hoc/Layout/Layout';

import Home from 'pages/Home/Home';
import Game from 'pages/Game/Game';

const App: React.FC = () => (
	<Layout>
		<Helmet>
			<title>Kubrick</title>
		</Helmet>

		<Routes>
			<Route path='/' element={<Home />} />
			<Route path='/play' element={<Game />} />
		</Routes>
	</Layout>
);

export default App;
