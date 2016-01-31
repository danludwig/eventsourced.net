import TopNav from './TopNav'
import Footer from './Footer'

export default class Layout extends React.Component {
  render() {
    const { children } = this.props
    return (
      <div>
        <TopNav />
        <div className="container body-content">
          {children}
          <Footer />
        </div>
      </div>
    )
  }
}
