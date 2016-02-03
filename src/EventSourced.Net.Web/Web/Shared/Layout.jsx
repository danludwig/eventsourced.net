import TopNav from './TopNav'
import Footer from './Footer'

const Layout = ({ children }) => (
  <div>
    <TopNav />
    <div className="container body-content">
      { children }
      <Footer />
    </div>
  </div>
)

Layout.propTypes = {
  children: React.PropTypes.object.isRequired,
}

export default Layout
