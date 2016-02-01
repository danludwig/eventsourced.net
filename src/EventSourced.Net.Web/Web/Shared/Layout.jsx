import TopNav from './TopNav'
import Footer from './Footer'

export default ({ children }) => (
  <div>
    <TopNav />
    <div className="container body-content">
      { children }
      <Footer />
    </div>
  </div>
)
